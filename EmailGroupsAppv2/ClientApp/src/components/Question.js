import React from 'react';
import { Modal, ModalBody, ModalFooter, Button } from 'reactstrap';
import PropTypes from 'prop-types';

export default function Question(props) {
    const { showModal, content, yesClicked, noClicked } = props;

    return (
        <Modal isOpen={showModal}>
            <ModalBody>
                {content}
            </ModalBody>
            <ModalFooter>
                <Button color={'secondary'} onClick={noClicked}>No</Button>
                <Button
                    color={'success'}
                    onClick={yesClicked}>
                    Yes
                </Button>
            </ModalFooter>
        </Modal>
    )
}

Question.propTypes = {
    showModal: PropTypes.bool,
    content: PropTypes.string.isRequired,
    yesClicked: PropTypes.func.isRequired,
    noClicked: PropTypes.func.isRequired
}