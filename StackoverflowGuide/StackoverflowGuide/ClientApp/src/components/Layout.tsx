import * as React from "react";
import { Container } from "react-bootstrap";
import { NavMenu } from "./NavMenu";

export default (props: { children?: React.ReactNode }) => (
  <div style={{ height: "100%" }}>
    <NavMenu />
    <Container className="h-100">{props.children}</Container>
  </div>
);
