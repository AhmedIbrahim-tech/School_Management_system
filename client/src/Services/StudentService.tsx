import axios, { AxiosError } from 'axios';
import { CreateStudent, Student, UpdateStudent } from '../Interface/Student';
import { GenericBaseResponse } from '../Interface/GenericBaseResponse';


const baseURL = 'https://localhost:44341/Api/v1/Student';

const handleApiError = (error: AxiosError<GenericBaseResponse<any>>) => {
  if (error.response) {
    const errorResponse = error.response.data;
    const errorMessage = errorResponse.errors?.join(', ') || 
                        errorResponse.message || 
                        'An error occurred';
    throw new Error(errorMessage);
  }
  throw new Error(error.message || 'Network error occurred');
};

const StudentService = {
  getAllStudents: async (): Promise<Student[]> => {
    try {
      const response = await axios.get<GenericBaseResponse<Student[]>>(`${baseURL}/List`);
      if (!response.data.succeeded) {
        throw new Error(response.data.message);
      }
      return response.data.data || [];
    } catch (error) {
      throw handleApiError(error as AxiosError<GenericBaseResponse<Student[]>>);
    }
  },

  addStudent: async (model: CreateStudent): Promise<CreateStudent> => {
    try {
      const response = await axios.post<GenericBaseResponse<CreateStudent>>(`${baseURL}/Create`, model);
      if (!response.data.succeeded) {
        throw new Error(response.data.message);
      }
      return response.data.data!;
    } catch (error) {
      throw handleApiError(error as AxiosError<GenericBaseResponse<CreateStudent>>);
    }
  },

  deleteStudent: async (id: number): Promise<boolean> => {
    try {
      const response = await axios.delete<GenericBaseResponse<boolean>>(`${baseURL}/Delete/${id}`);
      if (!response.data.succeeded) {
        throw new Error(response.data.message);
      }
      return response.data.succeeded;
    } catch (error) {
      throw handleApiError(error as AxiosError<GenericBaseResponse<boolean>>);
    }
  },

  updateStudent: async (id: number, model: UpdateStudent): Promise<UpdateStudent> => {
    try {
      const updateModel = { ...model, id };
      
      const response = await axios.put<GenericBaseResponse<UpdateStudent>>(`${baseURL}/Edit`, updateModel);
      if (!response.data.succeeded) {
        throw new Error(response.data.message);
      }
      return response.data.data!;
    } catch (error) {
      throw handleApiError(error as AxiosError<GenericBaseResponse<UpdateStudent>>);
    }
  }
};

export default StudentService;