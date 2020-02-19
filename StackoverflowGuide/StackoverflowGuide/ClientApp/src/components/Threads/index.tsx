import React, { FunctionComponent } from "react";
import {
  Container,
  Row,
  Col,
  Card,
  CardImg,
  CardText,
  CardBody,
  CardTitle,
  CardSubtitle,
  Button
} from "reactstrap";

export const ThreadsScreen: FunctionComponent = () => {
  return (
    <div className="d-flex align-items-center" style={{ height: "100%" }}>
      <Row>
        <Col>
          <div>
            <Card>
              <CardImg
                top
                width="100%"
                src="/assets/318x180.svg"
                alt="Card image cap"
              />
              <CardBody>
                <CardTitle>Card title</CardTitle>
                <CardSubtitle>Card subtitle</CardSubtitle>
                <CardText>
                  Some quick example text to build on the card title and make up
                  the bulk of the card's content.
                </CardText>
                <Button>Button</Button>
              </CardBody>
            </Card>
          </div>
        </Col>
        <Col>.col</Col>
        <Col>.col</Col>
      </Row>
    </div>
  );
};
