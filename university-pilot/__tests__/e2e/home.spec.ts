import { test, expect } from "@playwright/test";

test("test", async ({ page }) => {
  await page.goto("http://localhost:3000/");
  await expect(page).toHaveTitle("University Pilot");

  await expect(
    page.getByRole("heading", { name: "University Pilot" }),
  ).toBeVisible();
  await page.getByRole("link", { name: "Dowiedz się więcej" }).click();
  await expect(
    page.getByRole("heading", { name: "Jak To Działa?" }),
  ).toBeVisible();
});
