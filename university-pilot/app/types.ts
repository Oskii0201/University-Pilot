export interface Event {
  id: string;
  title: string;
  description: string;
  startTime: string;
  endTime: string;
  room?: string;
  lecturer?: string;
}
export interface Course {
  id: number;
  name: string;
}
export interface Group {
  id: string;
  name: string;
  courses: Course[];
}
export interface Semester {
  id: string;
  name: string;
  academicYear: string;
}
export type FetchResult<T> =
  | { success: true; data: T }
  | { success: false; error: string };
export interface GroupSet {
  id: string;
  name: string;
  createdAt: string;
  groups: Group[];
}
