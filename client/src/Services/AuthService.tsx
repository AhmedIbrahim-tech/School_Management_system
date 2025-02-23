import { AxiosError } from 'axios';
import axios from '../Config/axiosConfig';
import { GenericBaseResponse } from '../Interface/GenericBaseResponse';
import { SignIn } from '../Interface/Auth/SignIn';
import { JwtAuthResult } from '../Interface/Auth/JwtAuthResult';

const baseURL = 'https://localhost:44341/Api/v1/Authentication';

const AuthService = {
  signIn: async (credentials: SignIn): Promise<JwtAuthResult> => {
    try {
      const formData = new FormData();
      formData.append('userName', credentials.userName);
      formData.append('password', credentials.password);

      const response = await axios.post<GenericBaseResponse<JwtAuthResult>>(`${baseURL}/SignIn`, formData);
      
      if (!response.data.succeeded || !response.data.data) {
        throw new Error(response.data.message || 'Authentication failed');
      }

      const { accessToken, refreshToken } = response.data.data;

      if (!accessToken || !refreshToken) {
        throw new Error('Invalid authentication response');
      }

      localStorage.setItem('accessToken', accessToken);
      localStorage.setItem('refreshToken', JSON.stringify(refreshToken));
      
      return response.data.data;
      
    } catch (error) {
      const err = error as AxiosError<GenericBaseResponse<any>>;
      throw new Error(err.response?.data?.message || 'Authentication failed');
    }
  },

  logout: () => {
    localStorage.removeItem('accessToken');
    localStorage.removeItem('refreshToken');
  },

  getAccessToken: (): string | null => {
    return localStorage.getItem('accessToken');
  },

  isAuthenticated: (): boolean => {
    return !!localStorage.getItem('accessToken');
  }
};

export default AuthService;