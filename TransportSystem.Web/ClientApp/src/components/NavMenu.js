import React from 'react';
import {Link} from 'react-router-dom';
import {Glyphicon, Nav, Navbar, NavItem} from 'react-bootstrap';
import {LinkContainer} from 'react-router-bootstrap';
import './NavMenu.css';

export default props => (
    <Navbar inverse fixedTop fluid collapseOnSelect>
        <Navbar.Header>
            <Navbar.Brand>
                <Link to={'/'}>TransportSystem.Web</Link>
            </Navbar.Brand>
            <Navbar.Toggle/>
        </Navbar.Header>
        <Navbar.Collapse>
            <Nav>
                <LinkContainer to={'/'} exact>
                    <NavItem>
                        <Glyphicon glyph='home'/> Home
                    </NavItem>
                </LinkContainer>
                <LinkContainer to={'/passengers'}>
                    <NavItem>
                        <Glyphicon glyph='road'/> Passenger systems
                    </NavItem>
                </LinkContainer>
                <LinkContainer to={'/smo'}>
                    <NavItem>
                        <Glyphicon glyph='stats'/> Queueing model
                    </NavItem>
                </LinkContainer>
                <LinkContainer to={'/transports'}>
                    <NavItem>
                        <Glyphicon glyph='globe'/> Many types of transport
                    </NavItem>
                </LinkContainer>
                <LinkContainer to={'/info'}>
                    <NavItem>
                        <Glyphicon glyph='info-sign'/> Get info
                    </NavItem>
                </LinkContainer>
            </Nav>
        </Navbar.Collapse>
    </Navbar>
);
