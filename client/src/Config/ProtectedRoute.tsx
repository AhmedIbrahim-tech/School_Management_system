import { Navigate } from 'react-router-dom';
import AuthService from '../Services/AuthService';
import { JSX } from 'react';

interface ProtectedRouteProps {
  children: JSX.Element;
}

export const ProtectedRoute = ({ children }: ProtectedRouteProps) => {
  if (!AuthService.isAuthenticated()) {
    return <Navigate to="/login" />;
  }

  return children;
};