import "bootstrap/dist/css/bootstrap.css";

import * as React from "react";
import * as ReactDOM from "react-dom";
import { Provider } from "react-redux";
import { ConnectedRouter } from "connected-react-router";
import { store } from "./store";
import App from "./App";
import registerServiceWorker from "./registerServiceWorker";
import history from "./store/applcationHistory";

ReactDOM.render(
  <Provider store={store}>
    <ConnectedRouter history={history}>
      <App />
    </ConnectedRouter>
  </Provider>,
  document.getElementById("root")
);

registerServiceWorker();
