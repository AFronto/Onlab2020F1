import React, { FunctionComponent, useState } from "react";
import {
  Row,
  Col,
  Card,
  CardBody,
  CardTitle,
  Form,
  FormGroup,
  Label,
  Input,
  Button,
  FormFeedback
} from "reactstrap";

export const RegisterScreen: FunctionComponent = () => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [passwordRepeat, setPasswordRepeat] = useState("");
  const [validEmail, setValidEmail] = useState("");
  const [validPassword, setValidPassword] = useState("");
  const [validPasswordRepeat, setValidPasswordRepeat] = useState("");

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

  function validatePasswordRepeat(): boolean {
    if (password !== passwordRepeat) {
      setValidPasswordRepeat("has-danger");
      return false;
    } else {
      setValidPasswordRepeat("");
      return true;
    }
  }

  function submitRegister() {
    const vE = validateEmail();
    const vP = validatePassword();
    const vPR = validatePasswordRepeat();
    if (vE && vP && vPR) {
      console.log("Submit register");
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
                <CardTitle>Registration</CardTitle>
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
                    placeholder="New password"
                  />
                  <FormFeedback invalid>
                    <h6>The password must be at least 8 characters long!</h6>
                  </FormFeedback>
                </FormGroup>
                <FormGroup>
                  <h5>
                    <Label for="passwordRpeat">Repeat your Password:</Label>
                  </h5>
                  <Input
                    invalid={validPasswordRepeat === "has-danger"}
                    value={passwordRepeat}
                    onChange={e => setPasswordRepeat(e.target.value)}
                    type="password"
                    bsSize="lg"
                    placeholder="New password repeated"
                  />
                  <FormFeedback invalid>
                    <h6>The password must be identical to this!</h6>
                  </FormFeedback>
                </FormGroup>
                <Button
                  block
                  size="lg"
                  color="success"
                  onClick={submitRegister}
                >
                  Register
                </Button>
              </Form>
            </CardBody>
          </Card>
        </Col>
      </Row>
    </div>
  );
};
