import * as React from "react";
import { Route } from "react-router";
import Layout from "./components/Layout";
import { ThreadsScreen } from "./components/Thread";
import { LogInScreen } from "./components/Auth/LogIn";
import { RegisterScreen } from "./components/Auth/Register";

import "./custom.css";
import { AuthenticatedRoute } from "./routing/AuthenticatedRoute";

export default () => (
  <Layout>
    <Route path="/login" component={LogInScreen} />
    <Route path="/register" component={RegisterScreen} />
    <AuthenticatedRoute exact path="/">
      <ThreadsScreen />
    </AuthenticatedRoute>
    <AuthenticatedRoute path="/threads">
      <ThreadsScreen />
    </AuthenticatedRoute>
  </Layout>
);
