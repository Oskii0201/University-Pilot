import messages from "@/app/data/loadingMessages.json";

/**
 * Zwraca losową wiadomość w zależności od przeznaczenia.
 * @param type - Typ wiadomości: "upload" | "form" | "error"
 * @returns Losowy komunikat
 */
export const getRandomLoadingMessage = (
  type: "upload" | "form" | "error" | "success" = "error",
): string => {
  const messageList = messages[type] || messages["error"];
  return messageList[Math.floor(Math.random() * messageList.length)];
};
