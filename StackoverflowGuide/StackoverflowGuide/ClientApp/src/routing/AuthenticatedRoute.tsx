import React, { FunctionComponent } from "react";
import { Route, Redirect } from "react-router";
import { useDispatch } from "react-redux";
import { loadAuthData } from "../store/Auth";
import AuthData from "../data/server/Auth/AuthData";
import { isLoggedIn } from "../general_helpers/AuthHelper";

interface IProps {
  exact?: boolean;
  path: string;
  children: React.ReactNode;
}

export const AuthenticatedRoute: FunctionComponent<IProps> = ({
  children,
  ...rest
}) => {
  const dispatch = useDispatch();

  if (isLoggedIn()) {
    const jwt: AuthData = {
      token: localStorage.getItem("jwtToken")!,
      tokenExpirationTime: parseInt(
        localStorage.getItem("jwtTokenExpirationTime")!
      ),
      id: localStorage.getItem("jwtId")!,
    };
    dispatch(loadAuthData({ jwt: jwt }));
    return <Route {...rest} render={({ location }) => children} />;
  }

  return (
    <Route
      {...rest}
      render={({ location }) =>
        isLoggedIn() ? (
          children
        ) : (
          <Redirect
            to={{
              pathname: "/login",
              state: { from: location },
            }}
          />
        )
      }
    />
  );
};
