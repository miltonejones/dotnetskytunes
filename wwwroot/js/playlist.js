const API_ENDPOINT = "https://u8m0btl997.execute-api.us-east-1.amazonaws.com";

const getPlaylistGrid = async (page = 1) => {
  const response = await fetch(
    API_ENDPOINT + `/request/Title/ASC/${page}/playlist`
  );
  return await response.json();
};

const savePlaylist = async (json) => {
  const requestOptions = {
    method: "PUT",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(json),
  };
  const response = await fetch(`${API_ENDPOINT}/playlist`, requestOptions);
  return await response.json();
};

const updateList = async (playlist) => {
  const { related } = playlist;
  const verb =
    related.indexOf(editedTrack.fileKey) > -1 ? "removed from" : "added to";
  const updated = {
    ...playlist,
    related:
      related.indexOf(editedTrack.fileKey) > -1
        ? related.filter((f) => f !== editedTrack.fileKey)
        : related.concat(editedTrack.fileKey),
  };

  await savePlaylist(updated);

  showToast(`"${editedTrack.title}" ${verb} playlist "${playlist.Title}"`);

  updatePlaylistIcons();
  hidePlaylistEditor();
};

function showToast(message, title = "Success!") {
  const messageBox = document.getElementById("toast-message");
  const messageTitle = document.getElementById("toast-title");
  if (!messageBox) return;
  messageBox.innerHTML = message;
  messageTitle.innerHTML = title;
  const toast = new bootstrap.Toast(document.getElementById("liveToast"));
  toast.show();
}

let currentPlayLists = [];
let relatedTracks = [];

function saveToPlaylist(listKey) {
  const item = currentPlayLists.find((f) => f.listKey === listKey);
  if (!item) return alert("Could not find playlist " + listKey);
  updateList(item);
}

const setPhoto = async (listKey) => {
  const playlist = currentPlayLists.find((f) => f.listKey === listKey);
  const ok = confirm(
    `Set playlist ${playlist.Title} image to the album art from "${editedTrack.title}"?`
  );
  if (!ok) return;
  const updated = {
    ...playlist,
    image: editedTrack.albumImage,
    items: null,
    track: null,
  };
  hideMenu();
  await savePlaylist(updated);
  showToast(`"${updated.Title}" thumbnail updated!`);
  setTimeout(() => {
    location.reload();
  }, 2999);
};

function updateDrawerIcons() {
  getPlaylistGrid().then((data) => {
    currentPlayLists = data.records;

    document.querySelectorAll(".related").forEach((item) => {
      const listKey = item.getAttribute("data-listkey");
      const title = item.getAttribute("data-title");
      const list = currentPlayLists.find(
        (f) => f.listKey === listKey || f.Title === title
      );
      //   console.log({ list });
      if (list) {
        item.style.cursor = "pointer";
        item.classList.remove("fa-solid");
        item.classList.remove("fa-regular");
        const ok =
          list.related.indexOf(editedTrack.fileKey) > -1
            ? "fa-solid"
            : "fa-regular";

        item.classList.add(ok);
      } else {
        item.classList.add("fa-regular");
      }
    });
  });
}

function updatePlaylistIcons() {
  getPlaylistGrid().then((data) => {
    const relatedPlaylists = [];
    if (!data || !data.records) return relatedPlaylists;
    data.records.forEach((item) => {
      if (item.related && Array.isArray(item.related)) {
        relatedPlaylists.push(...item.related);
      }
    });
    relatedTracks = relatedPlaylists;
    currentPlayLists = data.records;

    document.querySelectorAll(".relatable").forEach((item) => {
      item.style.cursor = "pointer";
      item.classList.remove("fa-solid");
      item.classList.remove("fa-regular");
      const key = item.getAttribute("data-file-key");
      const ok = relatedTracks.indexOf(key) > -1 ? "fa-solid" : "fa-regular";

      item.classList.add(ok);
    });
  });
}

document.addEventListener("DOMContentLoaded", updatePlaylistIcons);

let draggedItem = null;
let dragOverIndex = null;

const setDraggedItem = (e) => (draggedItem = e);
const setDragOverIndex = (e) => (dragOverIndex = e);

const handleDragStart = (e, song, index) => {
  setDraggedItem({ song, index });
  e.dataTransfer.setData("text/plain", song.ID);
  e.dataTransfer.effectAllowed = "move";
};

const handleDragOver = (e, index) => {
  e.preventDefault();
  e.dataTransfer.dropEffect = "move";
  e.target.classList.add("drag-over");
  setDragOverIndex(index);
};

const handleDragLeave = (e) => {
  setDragOverIndex(null);
  e.target.classList.remove("drag-over");
};

function createKey(name) {
  return name.replace(/[\s&-]/g, "").toLowerCase();
}

const handleDrop = (e, dropIndex, listKey) => {
  e.preventDefault();

  console.log({ draggedItem });

  if (draggedItem && draggedItem.index !== dropIndex) {
    getPlaylistGrid().then((data) => {
      const list = data.records.find((f) => createKey(f.Title) === listKey);

      if (list) {
        console.log({ draggedItem2: draggedItem });

        const newRecords = [...list.related];
        const [movedItem] = newRecords.splice(draggedItem.index, 1);
        newRecords.splice(dropIndex, 0, movedItem);

        const updated = {
          ...list,
          items: null,
          track: null,
          related: newRecords,
        };

        console.log({ updated });

        savePlaylist(updated).then(() => {
          showToast(`"${updated.Title}" track order changed!`);
          setTimeout(() => {
            location.reload();
          }, 2999);
        });
      }

      setDraggedItem(null);
      setDragOverIndex(null);
    });
  }
};

function highlightRow(id) {
  document.querySelectorAll(".track-row").forEach((item) => {
    item.classList.remove("playing");
    const key = item.getAttribute("data-track-id");
    console.log({ id, key });
    if (id == key) {
      item.classList.add("playing");
    }
  });
}
