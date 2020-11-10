/*

import React from 'react';
import { BrowserRouter, Switch, Route } from 'react-router-dom';

import { routes } from 'routes';
import MainTemplate from 'templates/MainTemplate';

export const history = createBrowserHistory();

const Root = () => (

  <BrowserRouter history={history}>
    <MainTemplate>
      <Switch>
        <PrivateRoute user={user} exact path={routes.register} component={RegisterPage} />
        <PrivateRoute user={user} exact path={routes.activate} component={ActivatePage} />
        <Route path={routes.login} component={LoginPage} />
        <Route user={user} path={routes.forgotPassword} component={ForgotPassword} />

        <PrivateRoute user={user} path={routes.categories} component={Categories} />
        <PrivateRoute user={user} path={`${routes.category}/:id`} component={Category} />
        <PrivateRoute user={user} path={routes.addExcersise} component={AddExercise} />
        <PrivateRoute user={user} path={`${routes.editExcersise}/:id`} component={EditExercise} />
        <PrivateRoute user={user} path={`${routes.excersise}/:id`} component={Exercise} />

        <PrivateRoute user={user} path={routes.plans} component={Plans} />
        <PrivateRoute user={user} path={`${routes.plan}/:id`} component={Plan} />

        <PrivateRoute user={user} path={routes.users} component={AllUsers} />
        <PrivateRoute user={user} path={routes.organizations} component={Organizations} />

        <PrivateRoute user={user} path={routes.trainers} component={Trainers} />

        <PrivateRoute user={user} path={routes.clients} component={Clients} />
        <PrivateRoute user={user} path={`${routes.user}/:id`} accessRole={[1, 2, 3]} component={User} />
        <PrivateRoute user={user} path={`${routes.myProfile}/:id`} accessRole={[1, 2, 3]} component={MyProfile} />
      </Switch>
    </MainTemplate>
  </BrowserRouter>
);

export default Root;
*/

import React, {useState} from 'react';

import { BrowserRouter, Switch, Route } from 'react-router-dom';
import { mainTheme } from 'theme/mainTheme';
import { whiteTheme } from 'theme/whiteTheme';
import { ThemeProvider } from 'styled-components';
import { createBrowserHistory } from 'history';

import MainTemplate from 'templates/MainTemplate';
import { routes } from 'utils/routes';
import LoginPage from 'modules/Auth/LoginPage';
import ForgotPasswordPage from 'modules/Auth/ForgotPasswordPage';
import ResetPasswordPage from 'modules/Auth/ResetPasswordPage';
import ActivateAccountPage from 'modules/Auth/ActivateAccountPage';

import OrganizationUsers from 'modules/Users/OrganizationUsers';
import { PrivateRoute, Role } from '../utils/PrivateRoute';
import Alert from 'components/molecules/Alert';
import { ThemeContext } from '../support/context/ThemeContext';
import { LanguageContext } from '../support/context/LanguageContext';
import { userContext } from '../support/context/UserContext';
import { Categories } from '../modules/Exercises/Categories';
import { Category } from './Exercises/Category';

export const history = createBrowserHistory();



const Root = () => {

  const [theme] = useState(mainTheme)
  const [user] = useState(JSON.parse((localStorage.getItem('user'))));
  const [selectedLanguage] = useState('en');

  console.log(theme)
  

  return(
  <LanguageContext.Provider value={{lang: selectedLanguage}}>
  {/* is need to use theme context? */}
    <ThemeContext.Provider value={{theme}}>
    <ThemeProvider theme={theme}>
    <userContext.Provider value={{user}}>
    <BrowserRouter history={history}>
      <MainTemplate>
        <Switch>
          <Route path={routes.login} component={LoginPage} />
          <Route path={routes.forgotPassword} component={ForgotPasswordPage} />
          <Route path={routes.resetPassword} component={ResetPasswordPage} />
          <Route path={routes.activate} component={ActivateAccountPage} />
          <PrivateRoute roles={[Role.Owner]} path="/organizationusers" component={OrganizationUsers} />

          <PrivateRoute path={routes.categories} component={Categories} />
          <PrivateRoute path={routes.category} component={Category} />
        </Switch>
      </MainTemplate>
      </BrowserRouter>
    </userContext.Provider>
    </ThemeProvider>
    </ThemeContext.Provider>
    </LanguageContext.Provider>
  )
};

export default Root;
