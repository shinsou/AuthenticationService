import React, {Component, Fragment} from 'react';
import { Link } from 'react-router-dom';
import {withRouter} from 'react-router-dom';

//import MetisMenu from 'react-metismenu';
import {
    List,
    ListItem,
    ListItemIcon,
    ListItemText,
    Divider,
    Collapse,
    //IconButton,
    MenuList,
    MenuItem,
    Drawer
  } from '@material-ui/core';
  import {
     Home,
     Notifications,
     AccountCircle,
  } from '@material-ui/icons';
//import Divider from "@material-ui/core/Divider";
import ExpandMoreIcon from "@material-ui/icons/ExpandMore";
import ExpandLessIcon from "@material-ui/icons/ExpandLess";
//import Collapse from "@material-ui/core/Collapse";

import {MainNav, AuthenticationSetupNav, SystemNav} from './NavItems';

const navSections = [
    {
        header: 'Dashboard',
        nav: MainNav
    },
    {
        header: 'Authentication setup',
        nav: AuthenticationSetupNav
    },
    {
        header: 'System',
        nav: SystemNav,
        customStyles: {
            position: 'absolute',
            bottom: 20
        }
    },
];

class SidebarItem extends Component {
    constructor(props){
        super(props);
        
        this.state = {
            isCollapsable: props.content && props.content.length > 0,
            isOpen: this.setCollapsableState(props),
            node: props
        }

        this.activeRoute = this.activeRoute.bind(this);
        this.handleClick = this.handleClick.bind(this);
    }

    activeRoute(routeName) {
        return window.location.pathname.indexOf(routeName) > -1 ? true : false;    
    }

    setCollapsableState(node)
    {
        let isCollapsable = node.content && node.content.length > 0;
        var result = isCollapsable && this.recursiveFindPath(node);
        
        return result;
    }

    recursiveFindPath(node)
    {
        if (this.activeRoute(node.to)){
            return true;
        }

        var result = false;
        if(!node.content)
            return false;

        for(var i = 0; i < node.content.length; ++i)
        {
            result = this.recursiveFindPath(node.content[i]);
            if(result) {
                return true;
            }
        }

        return result;
    }

    handleClick(e){
        e.stopPropagation();
        
        if(this.state.isCollapsable){
            this.setState({ isOpen: !this.state.isOpen });
        }
    }

    render(){
        let menuItemCursorStyle = {
            cursor: 'pointer'
        };

        return(
            <li className="metismenu-item" onClick={this.handleClick} style={menuItemCursorStyle}>
                    {this.state.isCollapsable &&
                        <a className="metismenu-link">
                            <i className={"metismenu-icon" + (this.state.node.icon ? ` ${this.state.node.icon}` : "")} />
                            {this.state.node.label}
                            {this.state.isCollapsable && !this.state.isOpen
                                ? <i className="metismenu-state-icon pe-7s-angle-down" />
                                : null
                            }
                            {
                                this.state.isCollapsable && this.state.isOpen
                                ? <i className="metismenu-state-icon pe-7s-angle-down rotate-minus-90" />
                                : null
                            }
                        </a>}

                    {!this.state.isCollapsable &&
                        <Link to={this.state.node.to} className={"metismenu-link" + (this.activeRoute(this.state.node.to) ? " active" : "")}>
                            <i className={"metismenu-icon" + (this.state.node.icon ? ` ${this.state.node.icon}` : "")} />
                            {this.state.node.label}
                        </Link>
                    }
                    
                {this.state.isCollapsable
                    ?
                        <ul className={"metismenu-container" + (this.state.isOpen ? " visible": "")}>
                        {this.state.node.content.map((item, index) =>
                            <SidebarItem {...item} key={index} />
                        )}
                        </ul>
                    : null
                }
            </li>)
    }
}

class Nav extends Component {
    render() {
        return (
            navSections.map((section, sectionIndex) => {
                return (
                    <div key={sectionIndex} style={(section.customStyles ? section.customStyles : null)}>
                        <h5 className="app-sidebar__heading">{section.header}</h5>
                        <div className="metismenu vertical-nav-menu">
                            <ul className="metismenu-container">
                                {section.nav.map((navContent, navIndex) => {
                                    return (<SidebarItem {...navContent} key={navIndex} />);
                                })}
                            </ul>
                        </div>
                    </div>
                );
            })
        );
    }

    isPathActive(path) {
        return this.props.location.pathname.startsWith(path);
    }
}

export default withRouter(Nav);