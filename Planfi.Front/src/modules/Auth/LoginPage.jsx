import React, { useEffect, useState } from 'react';
import styled from 'styled-components';
import { routes } from 'routes';
import Label from 'components/atoms/Label';
import Input from 'components/molecules/Input';
import Button from 'components/atoms/Button';
import AuthTemplate from 'templates/AuthTemplate';
import ValidationHint from 'components/atoms/ErrorMessageForm';
import InputContainer from 'components/atoms/InputContainerForm';
import { userService } from 'services/userServices';
import { useHistory } from 'react-router-dom';
import { translate } from 'utils/Translation';
import { withRouter } from 'react-router-dom';
import {
  Formik, Field, Form,
} from 'formik';
import * as Yup from 'yup';
import Logo from 'components/atoms/Logo';
import { useNotificationContext, ADD } from 'support/context/NotificationContext';
import { useCookies } from 'react-cookie';
import { detectBrowser } from '../../utils/common.util';
import Loader from 'components/atoms/Loader';
import LoginHooks from './Google/LoginHooks';

const Link = styled.a`
  color: ${({ theme }) => theme.colorGray10};
  text-decoration: none;
  text-align: center;
  margin-top: 1.4rem;

  &:visited{
    color: ${({ theme }) => theme.colorGray10};
  }
`;

const initialValues = {
  email: '',
  password: '',
};

const timeToRedirectLogin = 1000;

const validationSchema = Yup.object({
  email: Yup.string().email(translate('EnterValidMail')).required(translate('ThisFieldIsRequired')),
  password: Yup.string().required(translate('ThisFieldIsRequired')),
});

const LoginPage = ({ setUser }) => {
  const { notificationDispatch } = useNotificationContext();
  const [cookies, setCookie, removeCookie] = useCookies(['cookie-name']);
  const [loading, setLoading] = useState(false);
  const history = useHistory();

  useEffect(() => {
    detectBrowser()
  }, []);

  const onSubmit = (values) => {
    const loginModelData = {
      email: values.email,
      password: values.password,
    };
    authenticateUser(loginModelData);
  };

  const redirectToPage = (data) => {
    setTimeout(() => {
      history.push(routes.myProfile);
    }, timeToRedirectLogin);
  }

  const saveJWTInCookies = (data) => {
    setCookie('JWT', data.token, { path: '/', })
  }

  const authenticateUser = (loginModelData) => {

    setLoading(true)
    userService
      .login(loginModelData)
      .then((data) => {
        saveJWTInCookies(data)
        redirectToPage(data);
        localStorage.removeItem('user');
        delete data.token;
        localStorage.setItem('user', JSON.stringify(data));
        setUser(data)
        setLoading(false)
      })
      .catch((error) => {
        notificationDispatch({
          type: ADD,
          payload: {
            content: { success: error, message: 'Api send error' },
            type: 'error',
          },
        });
        setLoading(false)
      });
  };

  if (loading) return <Loader isLoading={loading} />;

  return (
    <AuthTemplate>
      <Logo src="logo.png" />
      <Formik initialValues={initialValues} validationSchema={validationSchema} onSubmit={onSubmit} validateOnChange={false}>
        {({ errors, touched, isValid }) => (
          <Form>
            <InputContainer>
              <Label type="top" text={translate('YourMail')}>
                <Field type="email" name="email" placeholder={translate('EmailAddress')} as={Input} error={errors.email && touched.email} />
              </Label>
              <ValidationHint name="pUemail" />
            </InputContainer>

            <InputContainer>
              <Label type="top" text={translate('Password')}>
                <Field type="password" name="password" placeholder={translate('EnterPassword')} as={Input} error={errors.password && touched.password} />
              </Label>
              <ValidationHint name="password" />
            </InputContainer>
            <Button type="submit" buttonType="primary" size="lg" buttonPlace="auth">{translate('SignIn')}</Button>
          </Form>
        )}
      </Formik>
      <LoginHooks />
      <Link href={routes.forgotPassword}>{translate('ForgotPassword')}</Link>
    </AuthTemplate>
  );
};

export default withRouter(LoginPage);
