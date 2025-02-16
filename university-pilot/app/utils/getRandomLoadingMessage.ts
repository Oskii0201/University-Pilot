import messages from "@/app/data/loadingMessages.json";

/**
 * Zwraca losową wiadomość z `loadingMessages.json`
 */
export const getRandomLoadingMessage = (): string => {
  return messages[Math.floor(Math.random() * messages.length)];
};
