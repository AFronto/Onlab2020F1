import * as React from "react";
import { Container } from "reactstrap";
import NavMenu from "./NavMenu";

export default (props: { children?: React.ReactNode }) => (
  <div style={{ height: "100%" }}>
    <NavMenu />
    <Container style={{ height: "90%" }}>{props.children}</Container>
  </div>
);
