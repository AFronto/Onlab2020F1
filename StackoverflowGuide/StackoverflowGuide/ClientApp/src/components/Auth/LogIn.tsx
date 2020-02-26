import React, { FunctionComponent, useState } from "react";
import {
  Label,
  Input,
  Card,
  CardBody,
  CardTitle,
  Row,
  Col,
  Button,
  Form,
  FormGroup,
  FormFeedback
} from "reactstrap";

export const LogInScreen: FunctionComponent = () => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [validEmail, setValidEmail] = useState("");
  const [validPassword, setValidPassword] = useState("");

  function validateEmail(): boolean {
    const emailRex = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

    if (!emailRex.test(email)) {
      setValidEmail("has-danger");
      return false;
    } else {
      setValidEmail("");
      return true;
    }
  }

  function validatePassword(): boolean {
    if (password.length < 8) {
      setValidPassword("has-danger");
      return false;
    } else {
      setValidPassword("");
      return true;
    }
  }

  function submitLogIn() {
    const vE = validateEmail();
    const vP = validatePassword();
    if (vE && vP) {
      console.log("Submit log in");
    }
  }

  return (
    <div className="d-flex align-items-center" style={{ height: "100%" }}>
      <Row style={{ width: "100%", margin: 0 }}>
        <Col
          xs={{ size: "12", offset: "0" }}
          sm={{ size: "10", offset: "1" }}
          md={{ size: "8", offset: "2" }}
          lg={{ size: "6", offset: "3" }}
          xl={{ size: "4", offset: "4" }}
        >
          <Card outline color="primary">
            <CardBody>
              <h2>
                <CardTitle>Log In</CardTitle>
              </h2>
              <Form>
                <FormGroup>
                  <h5>
                    <Label for="email">Email address:</Label>
                  </h5>
                  <Input
                    invalid={validEmail === "has-danger"}
                    value={email}
                    onChange={e => setEmail(e.target.value)}
                    type="email"
                    bsSize="lg"
                    placeholder="Your email address"
                  />
                  <FormFeedback invalid>
                    <h6>This must be a legit email address!</h6>
                  </FormFeedback>
                </FormGroup>
                <FormGroup>
                  <h5>
                    <Label for="password">Password:</Label>
                  </h5>
                  <Input
                    invalid={validPassword === "has-danger"}
                    value={password}
                    onChange={e => setPassword(e.target.value)}
                    type="password"
                    bsSize="lg"
                    placeholder="Your password"
                  />
                  <FormFeedback invalid>
                    <h6>The password must be at least 8 characters long!</h6>
                  </FormFeedback>
                </FormGroup>
                <Button block size="lg" color="success" onClick={submitLogIn}>
                  Log In
                </Button>
              </Form>
            </CardBody>
          </Card>
        </Col>
      </Row>
    </div>
  );
};
