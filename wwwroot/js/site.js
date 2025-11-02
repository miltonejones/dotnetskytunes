function playTrackObject(trackId) {
  console.log("Playing track ID:", trackId);

  const track = trackList.find((t) => t.id === trackId);
  if (track && window.playGlobalTrack) {
    console.log("Found track:", track);
    window.playGlobalTrack(track, trackList, trackList.indexOf(track));
  } else {
    console.error("Track not found or player not available");
  }
}

async function announceChange(
  artist,
  title,
  ms,
  chatType,
  onSpeechStart = null,
  onSpeechEnd = null
) {
  if (ms < 150000) {
    return onSpeechEnd();
  }
  const requestOptions = {
    method: "POST",
    body: JSON.stringify({
      artist,
      title,
    }),
  };
  const response = await fetch(
    "https://ismvqzlyrf.execute-api.us-east-1.amazonaws.com/" + chatType,
    requestOptions
  );

  const json = await response.json();

  if (!json.messageContent) {
    return onSpeechEnd();
  }

  // Parse introduction from first message in choices.
  const { messageContent } = json;

  console.log({ json });

  const utterance = new SpeechSynthesisUtterance(messageContent);
  // Set up event listeners for speech start and end
  utterance.onstart = function (event) {
    console.log("Speech started");
    if (onSpeechStart && typeof onSpeechStart === "function") {
      onSpeechStart(event, messageContent);
    }
  };

  utterance.onend = function (event) {
    console.log("Speech ended");
    if (onSpeechEnd && typeof onSpeechEnd === "function") {
      onSpeechEnd(event, messageContent);
    }
  };

  // Optional: Configure voice properties
  utterance.rate = 1.0; // Speaking rate (0.1 to 10)
  utterance.pitch = 1.0; // Pitch (0 to 2)
  utterance.volume = 1.0; // Volume (0 to 1)
  utterance.lang = "en-US";

  // Speak the text
  window.speechSynthesis.speak(utterance);

  return true;
}

function doFallback(picture, src, fallback) {
  var image = new Image();
  image.onerror = () => {
    picture.src = fallback;
  };
  image.src = src;
}

function fixImages() {
  document.querySelectorAll(".fallback").forEach((item) => {
    var src = item.src;
    var fallback = item.getAttribute("data-fallback");
    // console.log({ src, fallback });
    doFallback(item, src, fallback);
  });
}

document.addEventListener("DOMContentLoaded", fixImages);

function updatePlaylistUI() {
  const playlistContainer = document.getElementById("playlistItems");
  if (!playlistContainer) return alert("Cannot find container for playlist");

  const state = window.audioState;

  console.log({ state });

  if (playlistContainer && state.trackList.length > 0) {
    var innerHTML = state.trackList
      .map(
        (track, index) => `
          <li class="list-group-item track-item ${
            index === state.currentTrackIndex ? "active" : ""
          } ${track.queued ? "queued" : ""}"
              onclick="window.playGlobalTrack(window.audioState.trackList[${index}], window.audioState.trackList, ${index})"
              style="cursor: pointer;">
              ${track.title} - ${track.artistName}
          </li>\n
      `
      )
      .join("");

    console.log({ innerHTML });
    // alert(innerHTML);
    playlistContainer.innerHTML = innerHTML;
  }
}
