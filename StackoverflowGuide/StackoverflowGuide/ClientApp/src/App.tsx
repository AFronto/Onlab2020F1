import * as React from "react";
import { Route } from "react-router";
import Layout from "./components/Layout";
import { ThreadsScreen } from "./components/Thread";
import { LogInScreen } from "./components/Auth/LogIn";
import { RegisterScreen } from "./components/Auth/Register";
import { SingleThreadScreen } from "./components/Thread/SingleThread";

import "./custom.css";
import "react-bootstrap-typeahead/css/Typeahead.css";
import { AuthenticatedRoute } from "./routing/AuthenticatedRoute";
import { NavMenu } from "./components/NavMenu";

export default () => (
  <div style={{ height: "100%" }}>
    <NavMenu />
    <Layout>
      <Route path="/login" component={LogInScreen} />
      <Route path="/register" component={RegisterScreen} />
      <AuthenticatedRoute exact path="/">
        <ThreadsScreen />
      </AuthenticatedRoute>
      <AuthenticatedRoute exact path="/threads">
        <ThreadsScreen />
      </AuthenticatedRoute>
      <AuthenticatedRoute path="/threads/:id">
        <SingleThreadScreen />
      </AuthenticatedRoute>
    </Layout>
  </div>
);
