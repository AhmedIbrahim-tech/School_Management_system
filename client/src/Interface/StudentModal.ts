import { Student } from "./Student";

export interface EditStudent {
    show: boolean;
    student: Student;
    onHide: () => void;
    onSuccess: () => void;
  }
  


  export interface AddStudent{
    show: boolean;
    onHide: () => void;
    onSuccess: () => void;
  }