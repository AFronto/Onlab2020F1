import * as React from "react";
import { Route } from "react-router";
import Layout from "./components/Layout";
import Home from "./components/Home";
import { Counter } from "./components/Counter";
import { ThreadsScreen } from "./components/Thread";
import { LogInScreen } from "./components/Auth/LogIn";
import { RegisterScreen } from "./components/Auth/Register";

import "./custom.css";

export default () => (
  <Layout>
    <Route exact path="/" component={Home} />
    <Route path="/counter" component={Counter} />
    <Route path="/login" component={LogInScreen} />
    <Route path="/register" component={RegisterScreen} />
    <Route path="/threads" component={ThreadsScreen} />
  </Layout>
);
