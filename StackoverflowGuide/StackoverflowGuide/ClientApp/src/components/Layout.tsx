import * as React from "react";
import { Container } from "react-bootstrap";

export default (props: { children?: React.ReactNode }) => (
  <Container className="h-100">{props.children}</Container>
);
