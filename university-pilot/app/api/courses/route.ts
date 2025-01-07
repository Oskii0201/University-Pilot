import { NextRequest, NextResponse } from "next/server";
import { readFile } from "fs/promises";

export async function GET(request: NextRequest) {
  const { searchParams } = new URL(request.url);
  const semesterId = searchParams.get("semesterId");

  if (!semesterId) {
    return NextResponse.json(
      { error: "Missing semesterId parameter" },
      { status: 400 },
    );
  }

  try {
    const courses = await readFile(`data/courses.json`, "utf-8");
    return NextResponse.json(JSON.parse(courses));

    // Docelowy kod:
    // const response = await apiClient.get("");
    // if (!response.ok) throw new Error("Failed to fetch courses");
    // const data = await response.json();
    // return NextResponse.json(data);
  } catch (error) {
    console.error("Error fetching courses:", error);
    return NextResponse.json(
      { error: "Failed to fetch courses" },
      { status: 500 },
    );
  }
}
