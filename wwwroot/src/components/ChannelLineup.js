import React, { Component } from 'react';
import { Container, Row, Col } from 'reactstrap';

import { TunerStatus } from './TunerStatus';
import { Channels } from './Channels';

export class ChannelLineup extends Component {
  render () {
    return (
      <Container>
        <Row>
          <Col xs="4">
            <TunerStatus />
          </Col>
          <Col xs="8">
            <Channels />
          </Col>
        </Row>
      </Container>
    );
  }
}
