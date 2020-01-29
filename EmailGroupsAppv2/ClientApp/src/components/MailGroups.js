import React, { useState, Fragment, useEffect } from 'react';
import { ListGroup, Button, Row, Col } from 'reactstrap';
import MailGroupEdit from './MailGroupEdit';
import axios from "axios";
import MailGroup from './MailGroup';

export default function MailGroups() {

    const [loading, setLoading] = useState(false);

    const [showGroupEditModal, setShowGroupEditModal] = useState(false);
    const toggleGroupEditModal = () => setShowGroupEditModal(!showGroupEditModal);

    const [currentlyOpenedGroup, setCurrentlyOpenedGroup] = useState('');

    const [mailGroups, setMailGroups] = useState([]);
    const removeMailGroupFromState = id => {
        const index = mailGroups.findIndex(x => x.id === id);
        if (index !== -1) {
            const newMailGroups = [...mailGroups.slice(0, index), ...mailGroups.slice(index + 1, mailGroups.length)];
            setMailGroups(newMailGroups);
        }
    }
    const updateMailGroupOnState = mailGroup => {
        const index = mailGroups.findIndex(x => x.id === mailGroup.id);
        if (index === -1) {
            setMailGroups([...mailGroups, mailGroup]);
        }
        else {
            const newMailGroups = [
                ...mailGroups.slice(0, index),
                mailGroup,
                ...mailGroups.slice(index + 1, mailGroups.length)
            ];
            setMailGroups(newMailGroups);
        }
    }
    useEffect(() => {
        const fillMailGroups = async () => {
            setLoading(true);
            let response = await axios.get('api/MailGroups');
            setMailGroups(response.data);
            setLoading(false);
        };
        fillMailGroups();
    }, []);

    if (loading)
        return (<h1>Loading...</h1>)
    else
        return (
            <Fragment>
                <Row>
                    <Col sm={12} style={{ fontSize: '32px' }}>
                        Your mail groups:
                    <Button
                            color="primary"
                            className="float-right"
                            onClick={toggleGroupEditModal}>
                            Add new
                    </Button>
                    </Col>
                </Row>

                <ListGroup>
                    {mailGroups.map((mailGroup, index) => {
                        return (
                            <MailGroup
                                key={index}
                                mailGroup={{ ...mailGroup }}
                                isOpen={currentlyOpenedGroup === mailGroup.name}
                                onClick={() => setCurrentlyOpenedGroup(mailGroup.name === currentlyOpenedGroup ? '' : mailGroup.name)}
                                onDeleteGroup={removeMailGroupFromState}
                                onGroupEdit={updateMailGroupOnState} />
                        )
                    })}
                </ListGroup>

                {showGroupEditModal &&
                    <MailGroupEdit
                        toggleModal={toggleGroupEditModal}
                        onGroupEdit={updateMailGroupOnState} />}

            </Fragment>
        )
}