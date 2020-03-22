import React, { FunctionComponent } from "react";
import { Navbar, Nav } from "react-bootstrap";

export const NavMenu: FunctionComponent = () => {
  return (
    <Navbar fixed="top" bg="dark" variant="dark" expand="sm">
      <Navbar.Brand className="ml-3" href="/">
        StackoverflowGuide
      </Navbar.Brand>
      <Navbar.Toggle aria-controls="basic-navbar-nav" />
      <Navbar.Collapse id="basic-navbar-nav">
        <Nav className="ml-auto mr-3">
          <Nav.Link href="/">Threads</Nav.Link>
        </Nav>
      </Navbar.Collapse>
    </Navbar>
  );
};
