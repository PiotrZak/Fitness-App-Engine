import React from 'react';
import styled from 'styled-components';
import Button from 'components/atoms/Button';
import { useHistory } from 'react-router-dom';
import AuthTemplate from 'templates/AuthTemplate';
import { translate } from 'utils/Translation';
import { useThemeContext } from 'support/context/ThemeContext';
import Icon from 'components/atoms/Icon';

const ConfirmationWrapper = styled.div`
    position: absolute;
    bottom: 50%;
    width:calc(100% - 40px);
    left:0;
    margin:0 20px 0 20px;
    text-align: center;
`;

const ConfirmationPage = (props) => {
  const history = useHistory();
  const { theme } = useThemeContext();
  const { message } = props.location.state;

  const redirectToLogin = () => {
    history.push({
      pathname: '/login',
    });
  };

  const renderConfirmation = () => {
    if (message == 'Activation') {
      return (
        <ConfirmationWrapper>
          <Icon name="check" fill={theme.colorInputActive} />
          <h2>{translate('AccountActivated')}</h2>
          <Button onClick={() => redirectToLogin()} type="submit" buttonType="primary" size="lg" buttonPlace="auth">{translate('ReturnToLogin')}</Button>
        </ConfirmationWrapper>
      );
    }
    if (message == 'ResetPassword') {
      return (
        <ConfirmationWrapper>
          <Icon name="check" fill={theme.colorInputActive} />
          <h2>{translate('PasswordResetted')}</h2>
          <Button onClick={() => redirectToLogin()} type="submit" buttonType="primary" size="lg" buttonPlace="auth">{translate('ReturnToLogin')}</Button>
        </ConfirmationWrapper>
      );
    }
  };

  return (
    <AuthTemplate>
      {renderConfirmation()}
    </AuthTemplate>
  );
};

export default ConfirmationPage;
