import './App.css';
import { Student } from './Components/Student';
import Navigation from './Components/Navigation';
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import { Home } from './Components/Home';
import 'bootstrap-icons/font/bootstrap-icons.css';
import { Login } from './Auth/Login';
import { ProtectedRoute } from './Config/ProtectedRoute';
import './Config/axiosConfig';

function App() {
  return (
    <BrowserRouter>
      <Navigation />
      <div className='container'>
        <Routes>
          <Route path="/login" element={<Login />} />
          <Route path='/' element={
            <ProtectedRoute>
              <Home />
            </ProtectedRoute>
          } />
          <Route path='/student' element={
            <ProtectedRoute>
              <Student />
            </ProtectedRoute>
          } />
        </Routes>
      </div>
    </BrowserRouter>
  );
}

export default App;