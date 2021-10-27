import React, { useState } from 'react';
import { createPortal } from 'react-dom';
import styled from 'styled-components';
import PropTypes from 'prop-types';
import Icon from 'components/atoms/Icon';
import ReactPlayer from 'react-player';

const Container = styled.div`
  height: 4.8rem;
  width: 4.8rem;
  border-radius: 2px;

  background: ${({ theme }) => theme.colorGray70};

  position: relative;
`;

const Circle = styled.div`
  height: 1.6rem;
  width: 1.6rem;

  //center X icon in Circle
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;

  background: ${({ theme }) => theme.colorErrorDefault};
  border-radius: 50%;

  position: absolute;
  top: -.5rem;
  right: -.5rem;
`;

const Image = styled.img`
  height: 4.8rem;
  width: 4.8rem;
  border-radius: 2px;
`;

const StyledOverlay = styled.div`
  width: 100%;
  height: 100vh;
  position: fixed;
  top: 0;
  left: 0;
  z-index: 999;
  background: ${({ theme }) => theme.colorGray70};
`;

const CloseIconContainer = styled.div`

`;

const VideoPlayer = (attachmentSrc) => (
  <StyledOverlay>
    <CloseIconContainer>
      <Icon name="union" size="2rem" cursorType="pointer" />
    </CloseIconContainer>
    <ReactPlayer
      url={attachmentSrc}
      controls
      light
      width="100%"
      height="80vh"
    />
  </StyledOverlay>
);

export const TYPE = {
  IMAGE: 'image',
  VIDEO: 'video',
};

const AttachmentPreview = ({
  attachmentSrc, complete, setID, remove, type, videoType,
}) => {
  const [attachment, setAttachment] = useState('img/blankImage.png');

  const setFullScreen = (attachmentSrc) => createPortal(<VideoPlayer attachmentSrc={attachmentSrc} />, document.body);

  if (complete && attachment !== attachmentSrc) {
    setAttachment(attachmentSrc);
  }

  // render image preview
  if (type === TYPE.IMAGE) {
    return (
      <Container id={setID}>
        <Circle onClick={remove} id={`img-prev-${setID}`}>
          <Icon name="union" size=".7rem" onClick={remove} id={`img-prev-${setID}`} />
        </Circle>
        <Image src={attachment} />
      </Container>
    );
  }
  // render video player preview
  return (
    <Container id={setID}>
      <Circle onClick={remove} id={`img-prev-${setID}`}>
        <Icon name="union" size=".7rem" onClick={remove} id={`img-prev-${setID}`} />
      </Circle>
      {/* // todo - add movie icon */}
    </Container>
  );
};

AttachmentPreview.propTypes = {
  attachmentSrc: PropTypes.string.isRequired,
  complete: PropTypes.bool.isRequired,
  type: PropTypes.oneOf([TYPE.VIDEO, TYPE.IMAGE]).isRequired,
  videoType: PropTypes.string,
  // setID: PropTypes.string.isRequired,
};

AttachmentPreview.defaultProps = {
  videoType: 'null',
};

export default AttachmentPreview;
