import React, { FunctionComponent } from "react";
import { Navbar, Nav, InputGroup, Button, Form } from "react-bootstrap";
import { isLoggedIn, logOut } from "../general_helpers/AuthHelper";
import { Link } from "react-router-dom";
import { ReduxState } from "../store";
import { FaArrowLeft, FaSearch } from "react-icons/fa";
import { useSelector, useDispatch } from "react-redux";
import { sendSearch } from "../api/Thread";
import * as yup from "yup";
import { useForm } from "react-hook-form";

export const NavMenu: FunctionComponent = () => {
  const activePath = useSelector(
    (state: ReduxState) => state.router.location.pathname
  );

  const open_thread = useSelector(
    (state: ReduxState) => state.single_thread.open_thread
  );

  const open_post = useSelector((state: ReduxState) => state.open_post);

  const router = useSelector((state: ReduxState) => state.router);

  const schema = yup.object({
    searchTerm: yup.string().required(),
  });

  const { register, handleSubmit, reset } = useForm({
    validationSchema: schema,
  });

  const dispatch = useDispatch();

  const handleSearch = handleSubmit((data) => {
    dispatch(
      sendSearch(open_thread.thread.id, { searchTerm: data.searchTerm })
    );
    reset();
  });

  return (
    <Navbar fixed="top" bg="dark" variant="dark" expand="sm">
      {open_thread.thread ? (
        <>
          <Link className="ml-3 navbar-brand" to="/threads">
            <FaArrowLeft className="mr-3" />
            {open_thread.thread.name}
          </Link>
          <Navbar.Toggle aria-controls="basic-navbar-nav" />
          <Navbar.Collapse id="basic-navbar-nav">
            <Form className="col-12" inline noValidate onSubmit={handleSearch}>
              <InputGroup
                className="col-lg-8 col-md-10 col-xs-col-12 mx-auto"
                size="lg"
              >
                <Form.Control
                  type="text"
                  color="primary"
                  defaultValue=""
                  name="searchTerm"
                  ref={register}
                  placeholder="Search keywords here..."
                />
                <InputGroup.Append>
                  <Button variant="secondary" type="submit">
                    <FaSearch />
                  </Button>
                </InputGroup.Append>
              </InputGroup>
            </Form>
          </Navbar.Collapse>
        </>
      ) : open_post.title ? (
        <>
          <Link
            className="ml-3 navbar-brand"
            to={router.location.pathname.split("/post")[0]}
          >
            <FaArrowLeft className="mr-3" />
            Open Question
          </Link>
        </>
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
