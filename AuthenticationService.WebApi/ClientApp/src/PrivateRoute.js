import React, { lazy } from 'react';
//import AuthService from './Services/AuthService';
import { Redirect, Route } from 'react-router-dom';
import { AuthenticationService } from './Services/AuthenticationService';


const PrivateRoute = ({component: Component, ...rest}) => (
        <Route
            {...rest}
            render = {
                props => {
                    // do authentication
                    const session = AuthenticationService.sessionValue;
                    debugger;
                    if(!session){
                        return <Redirect to={{ pathname: '/login', state: { from:  props.location} }} />
                    }

                    // do authorization

                    return <Component {...props} />
                }
            }
        />
    );

export default PrivateRoute;