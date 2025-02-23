import { Component } from 'react';
import { Nav, Navbar, Container, NavDropdown } from 'react-bootstrap';
import { NavLink, useNavigate } from 'react-router-dom';
import AuthService from '../Services/AuthService';

const Navigation = () => {
  const isAuthenticated = AuthService.isAuthenticated();
  const navigate = useNavigate();

  const handleLogout = () => {
    AuthService.logout();
    navigate('/login');
  };

  return (
    <Navbar bg="success" variant="dark" expand="lg" sticky="top">
      <Container>
        <Navbar.Brand href="/">
          <img
            src="./logo192.png"
            width="40"
            height="40"
            className="d-inline-block align-top"
            alt="Logo"
          />
          <span className="ms-2 text-white">School Management</span>
        </Navbar.Brand>
        <Navbar.Toggle aria-controls="basic-navbar-nav" />
        <Navbar.Collapse id="basic-navbar-nav">
          <Nav className="mx-auto">
            {isAuthenticated && (
              <>
                <NavLink
                  className={({ isActive }) =>
                    `nav-link ${isActive ? 'active' : ''} text-white fs-5 px-3`
                  }
                  to="/"
                >
                  Home
                </NavLink>
                <NavLink
                  className={({ isActive }) =>
                    `nav-link ${isActive ? 'active' : ''} text-white fs-5 px-3`
                  }
                  to="/student"
                >
                  Students
                </NavLink>
                <NavDropdown
                  title="More"
                  id="collasible-nav-dropdown"
                  className="text-white fs-5 px-3"
                >
                  <NavDropdown.Item as={NavLink} to="/about">
                    About
                  </NavDropdown.Item>
                  <NavDropdown.Item as={NavLink} to="/contact">
                    Contact
                  </NavDropdown.Item>
                </NavDropdown>
              </>
            )}
          </Nav>
          <Nav>
            {!isAuthenticated ? (
              <>
                <NavLink
                  className={({ isActive }) =>
                    `nav-link ${isActive ? 'active' : ''} text-white fs-5 px-3`
                  }
                  to="/login"
                >
                  Sign In
                </NavLink>
              </>
            ) : (
              <Nav.Link 
                className="text-white fs-5 px-3"
                onClick={handleLogout}
              >
                Logout
              </Nav.Link>
            )}
          </Nav>
        </Navbar.Collapse>
      </Container>
    </Navbar>
  );
};

export default Navigation;