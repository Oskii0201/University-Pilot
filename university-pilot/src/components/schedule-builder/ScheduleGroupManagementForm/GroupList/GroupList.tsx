import { Group, Course, FieldOfStudyGroup } from "@/app/types";
import EditableGroupList from "./EditableGroupList";
import ReadOnlyGroupList from "./ReadOnlyGroupList";

interface CommonProps {
  unassignedCourses: Course[];
}

type EditableProps = {
  readOnlyMode?: false;
  groups: Group[];
  handleEditGroupName: (key: string, newName: string) => void;
  handleRemoveCourseFromGroup: (key: string, courseName: string) => void;
  handleAddCourseToGroup: (key: string, courseName: string) => void;
  handleRemoveGroup: (key: string, courses: Course[]) => void;
};

type ReadOnlyProps = {
  readOnlyMode: true;
  groups: FieldOfStudyGroup[];
};

type GroupListProps = CommonProps & (EditableProps | ReadOnlyProps);

export default function GroupList(props: GroupListProps) {
  if (props.readOnlyMode === true) {
    return <ReadOnlyGroupList {...props} />;
  } else {
    return <EditableGroupList {...props} />;
  }
}
