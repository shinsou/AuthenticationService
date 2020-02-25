import React, { Component, lazy } from 'react';
//import AuthService from './Services/AuthService';
import { Redirect, Route } from 'react-router-dom';
import { AuthenticationService } from './Services/AuthenticationService';
import { ThemeConsumer } from 'styled-components';

class PrivateRoute extends Component {
    constructor(props) {
        super(props);
        this.state = {
            loaded: false,
            loading: false,
            isAuthenticated: false
        }
    }

    componentDidMount(){
        this.authenticate();
    }

    authenticate(){
        const { loading } = this.state;
        if(!loading){
            this.setState({loading: true});
            AuthenticationService.isAuthenticated()
            .then(response => {
                this.setState({loading: false, loaded: true, isAuthenticated: response});
            });
        }
    }

    render() {
        const { component: Component, ...rest } = this.props;
        const { loaded, isAuthenticated} = this.state
        
        if(!loaded){
            return null;
        }
        
        return (
            <Route
                {...rest}
                render={
                    props => {
                        return isAuthenticated 
                            ? (<Component {...props} />)
                            : (<Redirect to={{ pathname: '/login', state: { from:  this.props.location} }} />)
                    }
                }
                />
        )

        // if(!isAuthenticated)
        // {
        //     return (<Redirect to={{ pathname: '/login', state: { from:  this.state.props.location} }} />);
        // }

        // return (<Component {...this.state.props} />)
    }
}

// const PrivateRoute = ({component: Component, ...rest}) => {
//     return (
//         <Route
//             {...rest}
//             render = {
//                 props => {
//                     // do authentication
//                     let da = AuthenticationService.isAuthenticated();
//                     const session = AuthenticationService.sessionValue;
//                     debugger;
//                     if(!session){
//                         return <Redirect to={{ pathname: '/login', state: { from:  props.location} }} />
//                     }

//                     // do authorization

//                     return <Component {...props} />
//                 }

//             }
//         />
//     )
// };

export default PrivateRoute;