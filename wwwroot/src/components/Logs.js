import React, { Component } from 'react';
import { Container, ListGroup, ListGroupItem } from 'reactstrap';

export class Logs extends Component {
    constructor(props) {
        super(props);
        this.state = { logs: [] };
    }

    render() {
        var logs = this.state.logs;

        return (
            <Container>
                <ListGroup>
                    {logs.map(entry => {
                        <ListGroupItem>{entry.message}</ListGroupItem>
                    })}
                </ListGroup>
            </Container>
        );
    }
};