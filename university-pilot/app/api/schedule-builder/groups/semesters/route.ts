import { NextResponse } from "next/server";
import { readFile } from "fs/promises";

export async function GET() {
  try {
    const semesters = await readFile("data/semesters.json", "utf-8");
    return NextResponse.json(JSON.parse(semesters));

    // Docelowy kod:
    // const response = await apiClient.get("");
    // if (!response.ok) throw new Error("Failed to fetch semesters");
    // const data = await response.json();
    // return NextResponse.json(data);
  } catch (error) {
    console.error("Error fetching semesters:", error);
    return NextResponse.json(
      { error: "Failed to fetch semesters" },
      { status: 500 },
    );
  }
}
