import React from 'react';
import Input from 'components/molecules/Input';
import styled from 'styled-components';
import PropTypes from 'prop-types';

const SearchContainer = styled.div`
margin-bottom:1.6rem;
`;

const StyledInput = styled(Input)`
  width: 100%;
`;

const Search = ({ callBack, placeholder, typeInput }) => (
  <SearchContainer>
  <StyledInput typeInput= {typeInput ? typeInput : "left"} icon="search" onChange={callBack} placeholder={placeholder} />
  </SearchContainer>
);

Search.propTypes = {
  callBack: PropTypes.func.isRequired,
  placeholder: PropTypes.string.isRequired,
};

export default Search;
