import React, { Component } from 'react';

import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'

import { faInfoCircle, faExclamationCircle, faTimesCircle,
    faSync, faTimes } from '@fortawesome/free-solid-svg-icons'

import { Badge, Button, ButtonGroup, Card, CardTitle, CardBody,
         Container, Col, Pagination, PaginationItem, PaginationLink, Row, Table } from 'reactstrap';

import humanize from 'humanize-duration';

export class Logs extends Component {
    constructor(props) {
        super(props);
        this.state = {
            logs: [],
            position: 1,
            pages: 1
        };

        this.createLogEntry = this.createLogEntry.bind(this);
        this.getData = this.getData.bind(this);
        this.paginationButtonClicked = this.paginationButtonClicked.bind(this);
    }

    componentDidMount() {
        this.getData();
    }

    getData() {
        fetch(`/status/logs/${this.state.position}`)
            .then(resp => resp.json())
            .then(data => this.setState({logs: data.logs, pages: data.pages}));
    }

    paginationButtonClicked(position) {
        if(position < 1 || position > this.state.pages)
            return;

        this.setState({position: position});
        this.getData();
    }

    clearLogs() {
        fetch('/status/logs', {method: 'DELETE'})
            .then(resp => {
                if(resp.status === 200) {
                    this.getData();
                }
            })
    }

    createLogEntry(entry) {

        var icon = faInfoCircle;
        var color = 'DodgerBlue';

        var now = Date.now();
        var date = Date.parse(entry.timestamp);

        switch(entry.level) {
            case 'Warn':
                icon = faExclamationCircle;
                color = 'yellow';
                break;
            case 'Fatal':
            case 'Error':
                icon = faTimesCircle;
                color = 'red';
                break;
            default:
                break;
        }

        return (
            <tr key={entry.id}>
                <td><FontAwesomeIcon icon={icon} color={color}/></td>
                <td><Badge>{entry.logger.replace('Relay.','')}</Badge></td>
                <td><code>{entry.message}</code></td>
                <td><small>{humanize(now - date, {largest: 1, round: true})}</small></td>
            </tr>
        );
    }

    render() {
        const items = []
        for(const i of Array(this.state.pages).keys())
        {
            const number = i + 1;
            items.push(
                <PaginationItem active={this.state.position === number} key={number}>
                    <PaginationLink onClick={() => this.paginationButtonClicked(number)}>{number}</PaginationLink>
                </PaginationItem>);
        }

        return (
            <Container>
                <Card>
                    <CardBody>
                    <CardTitle>Logs</CardTitle>
                    <Row>
                        <Col>
                            <Pagination size='sm'>
                                <PaginationItem disabled={this.state.position === 1}>
                                    <PaginationLink previous onClick={() => this.paginationButtonClicked(this.state.position-1)}/>
                                </PaginationItem>
                                {items}
                                <PaginationItem disabled={this.state.position === (this.state.pages)}>
                                    <PaginationLink next onClick={() => this.paginationButtonClicked(this.state.position+1)}/>
                                </PaginationItem>
                            </Pagination>
                        </Col>
                        <Col>
                            <ButtonGroup className='float-right' size='sm'>
                                <Button onClick={() => this.getData()}>
                                    <FontAwesomeIcon icon={faSync}/> Refresh</Button>
                                <Button color='danger' onClick={() => this.clearLogs()}>
                                    <FontAwesomeIcon icon={faTimes}/> Clear</Button>
                            </ButtonGroup>
                        </Col>
                    </Row>
                        <Table size='sm' striped hover borderless>
                            <thead>
                                <tr>
                                    <th></th>
                                    <th>Component</th>
                                    <th>Message</th>
                                    <th>Time</th>
                                </tr>
                            </thead>
                            <tbody>
                                {this.state.logs.map(this.createLogEntry)}
                            </tbody>
                        </Table>
                </CardBody>
            </Card>
        </Container>
        );
    }
};