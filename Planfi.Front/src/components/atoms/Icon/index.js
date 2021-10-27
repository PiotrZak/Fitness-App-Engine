import React from 'react';
import styled from 'styled-components';
import PropTypes from 'prop-types';

const IconsCode = {
  'ellipsis-h': 'e900',
  'angle-down': 'e90d',
  'angle-up': 'e90e',
  'arrow-circle-right': 'e90f',
  'arrow-left': 'e910',
  'arrow-right': 'e911',
  check: 'e912',
  'check-circle': 'e913',
  'clipboard-notes': 'e914',
  draggabledots: 'e915',
  dumbbell: 'e916',
  'exclamation-triangle': 'e901',
  heart: 'e902',
  'image-plus': 'e903',
  'list-ul': 'e904',
  'minus-circle': 'e905',
  paperclip: 'e906',
  plus: 'e907',
  'plus-circle': 'e908',
  'question-circle': 'e909',
  search: 'e90a',
  union: 'e90b',
  'user-circle': 'e90c',
  cog: 'e918',
  'image-slash': 'e919',
};

const FontIcon = styled.span`
    font-family: 'icomoon', sans-serif;
    font-size: ${({ size }) => size};
    color: ${({ fill }) => fill};
    cursor: ${({ cursorType }) => cursorType};
    speak: none;

    :before{
      ${({ name }) => `content: "\\${IconsCode[name]}"`};
    }
  `;

const Icon = ({
  size, fill, name, cursorType, ...rest
// eslint-disable-next-line react/jsx-props-no-spreading
}) => (
  <FontIcon size={size} fill={fill} name={name} cursorType={cursorType} {...rest} />
);

Icon.propTypes = {
  name: PropTypes.oneOf([
    'ellipsis-h', 'angle-down', 'angle-up', 'arrow-circle-right', 'arrow-left',
    'arrow-right', 'check', 'check-circle', 'clipboard-notes', 'draggabledots',
    'dumbbell', 'exclamation-triangle', 'heart', 'image-plus',
    'list-ul', 'minus-circle', 'paperclip', 'plus', 'plus-circle',
    'question-circle', 'search', 'union', 'user-circle',
    'cog', 'image-slash',
  ]).isRequired,
  size: PropTypes.string,
  fill: PropTypes.string,
  cursorType: PropTypes.string,
};

Icon.defaultProps = {
  size: '1.5rem',
  fill: ({ theme }) => theme.colorPrimary,
  cursorType: 'pointer',
};

export default Icon;
