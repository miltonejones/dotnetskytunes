const API_URL = "https://u8m0btl997.execute-api.us-east-1.amazonaws.com";
const getAppleLookup = async (searchTerm) => {
  const response = await fetch(
    `${API_URL}/apple/${encodeURIComponent(searchTerm)}`
  );
  return await response.json();
};

let appleTrack = {};

function performAppleLookup() {
  const form = document.getElementById("trackEditForm");
  const formData = new FormData(form);
  const formMessage = document.getElementById("formMessage");
  const searchButton = document.getElementById("searchButton");
  const searchLoading = document.getElementById("searchLoading");
  const searchText = document.getElementById("searchText");

  // Show loading state
  searchLoading.classList.remove("d-none");
  searchText.classList.add("d-none");
  searchButton.disabled = true;
  formMessage.classList.add("d-none");

  const searchData = {
    Title: formData.get("Title"),
    discNumber: formData.get("discNumber"),
    trackNumber: formData.get("trackNumber"),
    artistName: formData.get("artistName"),
    albumName: formData.get("albumName"),
    Genre: formData.get("Genre"),
    ID: formData.get("ID"),
  };

  const searchTerm = formData.get("Title") + " " + formData.get("artistName");

  getAppleLookup(searchTerm)
    .then((data) => {
      onAppleLookupSuccess(data);
    })
    .catch((error) => {
      onAppleLookupFailure(error);
    });
}

function onAppleLookupSuccess(data) {
  const resultsContainer = document.getElementById("searchResults");
  const emptyState = document.getElementById("emptyState");
  const formMessage = document.getElementById("formMessage");

  // Hide loading state
  document.getElementById("searchLoading").classList.add("d-none");
  document.getElementById("searchText").classList.remove("d-none");
  document.getElementById("searchButton").disabled = false;

  //   alert(JSON.stringify(data, 0, 2));

  if (data.results && data.results.length > 0) {
    // Populate results
    resultsContainer.innerHTML = data.results
      .map(
        (item, index) => `
            <div class="col-md-6 col-lg-4">
                <div class="card h-100 cursor-pointer hover-shadow"
                     onclick="handleAppleClick(${JSON.stringify(item).replace(
                       /"/g,
                       "&quot;"
                     )})">
                    <div class="card-body p-1 d-flex align-items-center" style="padding:0">
                        <img src="${item.artworkUrl100}" alt="${
          item.trackName
        }" 
                             class="img-thumbnail me-3" style="width: 60px; height: 60px; object-fit: cover;">
                        <div class="flex-grow-1" style="min-width: 0;">
                            <h6 class=" text-truncate mb-1">${
                              item.trackName || "Unknown Title"
                            }</h6>
                            <p class=" text-muted text-truncate small mb-1">${
                              item.artistName || "Unknown Artist"
                            }</p>
                            <p class=" text-muted text-truncate small">${
                              item.collectionName || "Unknown Album"
                            }</p>
                        </div>
                    </div>
                </div>
            </div>
        `
      )
      .join("");

    emptyState.classList.add("d-none");
    resultsContainer.classList.remove("d-none");
  } else {
    // Show error message
    formMessage.textContent = data.message || "Search failed";
    formMessage.className = `alert ${
      data.success ? "alert-success" : "alert-danger"
    }`;
    formMessage.classList.remove("d-none");
  }
}

function onAppleLookupFailure(error) {
  const formMessage = document.getElementById("formMessage");
  formMessage.textContent = "Search request failed: " + error.message;
  formMessage.className = "alert alert-danger";
  formMessage.classList.remove("d-none");

  // Hide loading state
  document.getElementById("searchLoading").classList.add("d-none");
  document.getElementById("searchText").classList.remove("d-none");
  document.getElementById("searchButton").disabled = false;
}

// async function handleAppleClick(track) {
//   alert(JSON.stringify(track, null, 2));
//   try {
//     const trackId = track.id;
//     const response = await fetch('@Url.Action("UpdateTrack", "Tracks")', {
//       method: "POST",
//       headers: {
//         "Content-Type": "application/json",
//         RequestVerificationToken: document.querySelector(
//           'input[name="__RequestVerificationToken"]'
//         ).value,
//       },
//       body: JSON.stringify({
//         itunesTrack: track,
//         trackId: trackId,
//       }),
//     });

//     const result = await response.json();
//     if (result.success) {
//       showToast("Track updated successfully!");
//       closeTrackEdit();
//       // Optional: refresh parent page data
//       if (window.refreshTracks) window.refreshTracks();
//     } else {
//       showToast("Error updating track: " + result.message, "error");
//     }
//   } catch (error) {
//     showToast("Error updating track: " + error.message, "error");
//   }
// }

function closeTrackEdit() {
  // Close the offcanvas
  const offcanvas = document.getElementById("trackEditDrawer");
  if (offcanvas) {
    const bsOffcanvas = bootstrap.Offcanvas.getInstance(offcanvas);
    bsOffcanvas.hide();
  }
}

// Add hover effects for cards
document.addEventListener("DOMContentLoaded", function () {
  const style = document.createElement("style");
  style.textContent = `
        .cursor-pointer { cursor: pointer; }
        .hover-shadow:hover { box-shadow: 0 0.5rem 1rem rgba(0, 0, 0, 0.15) !important; }
    `;
  document.head.appendChild(style);
});

const handleAppleClick = async (track) => {
  try {
    const trackItem = iTunesConvert(track);
    await getAlbum(track, trackItem);
  } catch (error) {
    alert("Error updating track: " + error.message);
  }
};

const getAlbumorArtistId = async (type, name, image) => {
  const response = await fetch(`${API_URL}/find`, {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify({ type, name, image }),
  });
  return await response.json();
};

const updateTable = async (track) => {
  const response = await fetch(`${API_URL}/update/s3Music`, {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(track),
  });
  return await response.json();
};

const getAlbum = async (itunes, track) => {
  const albumImage = itunes.artworkUrl100;
  const albumName = itunes.collectionName;
  const albumData = await getAlbumorArtistId("album", albumName, albumImage);

  const updatedTrack = {
    ...track,
    albumFk: albumData,
  };

  await getArtist(itunes, updatedTrack);
};

const getArtist = async (itunes, track) => {
  const albumImage = itunes.artworkUrl100;
  const artistName = itunes.artistName;
  const artistData = await getAlbumorArtistId("artist", artistName, albumImage);

  const updatedTrack = {
    ...track,
    artistFk: artistData,
  };

  await sendToAPI(updatedTrack);
};

const sendToAPI = async (track) => {
  const result = await updateTable(stripTrack(track));
  //   setITunesResults([]);
  //   onComplete && onComplete();

  const offcanvas = new bootstrap.Offcanvas(
    document.getElementById("trackEditDrawer")
  );
  offcanvas.hide();
  showToast("Track details updated");
  setTimeout(() => {
    location.reload();
  }, 2999);
};

const stripTrack = (track) => {
  const {
    Genre,
    Title,
    albumFk,
    artistFk,
    discNumber,
    trackNumber,
    ID,
    albumImage,
    trackTime,
  } = track;
  return {
    Genre,
    Title,
    albumFk,
    artistFk,
    discNumber,
    trackNumber,
    ID,
    trackTime,
    albumImage,
  };
};

const iTunesConvert = (itunes) => {
  return {
    ...appleTrack,
    Title: itunes.trackName,
    trackId: itunes.trackId,
    ID: appleTrack.id,
    albumName: itunes.collectionName,
    albumImage: itunes.artworkUrl100,
    Genre: itunes.primaryGenreName,
    genreKey: createKey(itunes.primaryGenreName),
    discNumber: itunes.discNumber,
    trackTime: itunes.trackTimeMillis,
    trackNumber: itunes.trackNumber,
    artistName: itunes.artistName,
    explicit: false,
  };
};
