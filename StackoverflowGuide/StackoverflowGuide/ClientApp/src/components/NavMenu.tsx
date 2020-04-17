import React, { FunctionComponent } from "react";
import { Navbar, Nav } from "react-bootstrap";
import { isLoggedIn, logOut } from "../general_helpers/AuthHelper";
import { Link } from "react-router-dom";
import { ReduxState } from "../store";
import { FaArrowLeft } from "react-icons/fa";
import { useSelector, useDispatch } from "react-redux";
import { loadSingleThread } from "../store/Thread/OpenThread";

export const NavMenu: FunctionComponent = () => {
  const activePath = useSelector(
    (state: ReduxState) => state.router.location.pathname
  );

  const open_thread = useSelector((state: ReduxState) => state.open_thread);

  const dispatch = useDispatch();

  return (
    <Navbar fixed="top" bg="dark" variant="dark" expand="sm">
      {open_thread.thread ? (
        <Link className="ml-3 navbar-brand" to="/threads">
          <FaArrowLeft className="mr-3" />
          {open_thread.thread.name}
        </Link>
      ) : (
        <>
          <Navbar.Brand className="ml-3" href="/">
            StackoverflowGuide
          </Navbar.Brand>
          <Navbar.Toggle aria-controls="basic-navbar-nav" />
          <Navbar.Collapse id="basic-navbar-nav">
            {isLoggedIn() ? (
              <Nav className="ml-auto mr-3">
                <Link
                  className={
                    ["/", "/threads"].includes(activePath)
                      ? "nav-link active"
                      : "nav-link"
                  }
                  to="/threads"
                >
                  Threads
                </Link>
                <Link
                  className={
                    activePath === "/profile" ? "nav-link active" : "nav-link"
                  }
                  to="/profile"
                >
                  Profile
                </Link>
                <Nav.Link onClick={() => logOut(dispatch)}>Log Out</Nav.Link>
              </Nav>
            ) : (
              <Nav className="ml-auto mr-3">
                <Link
                  className={
                    activePath === "/register" ? "nav-link active" : "nav-link"
                  }
                  to="/register"
                >
                  Register
                </Link>
                <Link
                  className={
                    activePath === "/login" ? "nav-link active" : "nav-link"
                  }
                  to="/login"
                >
                  Log In
                </Link>
              </Nav>
            )}
          </Navbar.Collapse>
        </>
      )}
    </Navbar>
  );
};
