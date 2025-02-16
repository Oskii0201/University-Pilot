export interface Event {
  id: string;
  title: string;
  description: string;
  startTime: string;
  endTime: string;
  room?: string;
  lecturer?: string;
}
export type Course = string;
export interface Group {
  groupId: number;
  groupName: string;
  assignedFieldsOfStudy: Course[];
}
export interface Semester {
  id: number;
  name: string;
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
