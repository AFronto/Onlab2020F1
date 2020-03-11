import React, { FunctionComponent } from "react";
import { Card, Form, Col, Button, Row } from "react-bootstrap";
import * as yup from "yup";
import { useForm } from "react-hook-form";
import { isArray } from "util";
import { createNewAccount } from "../../api/Auth";
import { useDispatch } from "react-redux";

export const RegisterScreen: FunctionComponent = () => {
  const schema = yup.object({
    email: yup
      .string()
      .email()
      .required(),
    password: yup
      .string()
      .min(8)
      .required(),
    passwordConfirmation: yup
      .string()
      .oneOf([yup.ref("password"), null], "Passwords must match")
  });

  const { register, handleSubmit, errors } = useForm({
    validationSchema: schema
  });

  const dispatch = useDispatch();

  const onSubmit = handleSubmit(data => {
    dispatch(
      createNewAccount({
        email: data.email,
        password: data.password,
        repeatPassword: data.passwordConfirmation
      })
    );
  });

  return (
    <Row className="h-100 align-items-center justify-content-center">
      <Card
        style={{ width: "18rem", marginTop: 100, marginBottom: 100 }}
        bg={"dark"}
        text={"white"}
      >
        <Card.Header as="h4">Create a new Account!</Card.Header>
        <Card.Body>
          <Form noValidate onSubmit={onSubmit}>
            <Form.Row>
              <Form.Group as={Col} controlId="emailField">
                <Form.Label>
                  <h5>Email:</h5>
                </Form.Label>
                <Form.Control
                  size="lg"
                  type="text"
                  name="email"
                  ref={register}
                  isInvalid={!!errors.email}
                />
                <Form.Control.Feedback type="invalid">
                  <h6>
                    {errors.email
                      ? isArray(errors.email)
                        ? errors.email[0].message
                        : errors.email.message
                      : ""}
                  </h6>
                </Form.Control.Feedback>
              </Form.Group>
            </Form.Row>
            <Form.Row>
              <Form.Group as={Col} controlId="passwordField">
                <Form.Label>
                  <h5>Password:</h5>
                </Form.Label>
                <Form.Control
                  size="lg"
                  type="password"
                  name="password"
                  ref={register}
                  isInvalid={!!errors.password}
                />
                <Form.Control.Feedback type="invalid">
                  <h6>
                    {errors.password
                      ? isArray(errors.password)
                        ? errors.password[0].message
                        : errors.password.message
                      : ""}
                  </h6>
                </Form.Control.Feedback>
              </Form.Group>
            </Form.Row>
            <Form.Row>
              <Form.Group as={Col} controlId="passwordConfirmationField">
                <Form.Label>
                  <h5>Repeate your password:</h5>
                </Form.Label>
                <Form.Control
                  size="lg"
                  type="password"
                  name="passwordConfirmation"
                  ref={register}
                  isInvalid={!!errors.passwordConfirmation}
                />
                <Form.Control.Feedback type="invalid">
                  <h6>
                    {errors.passwordConfirmation
                      ? isArray(errors.passwordConfirmation)
                        ? errors.passwordConfirmation[0].message
                        : errors.passwordConfirmation.message
                      : ""}
                  </h6>
                </Form.Control.Feedback>
              </Form.Group>
            </Form.Row>
            <Button type="submit" size="lg" block>
              Create Account
            </Button>
          </Form>
        </Card.Body>
      </Card>
    </Row>
  );
};
