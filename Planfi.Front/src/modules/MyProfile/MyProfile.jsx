import React, {
  useState, useEffect,
} from 'react';
import styled, { css } from 'styled-components';
import { UserInfo } from 'components/molecules/UserInfo/UserInfo';
import { useThemeContext } from 'support/context/ThemeContext';
import { userService } from 'services/userServices';
import { MyProfilePanel } from 'modules/MyProfile/MyProfilePanel';
import EditUserPasswordModal from 'modules/MyProfile/EditProfile/EditUserPassword';
import EditUserEmailModal from 'modules/MyProfile/EditProfile/EditUserEmail';
import EditUserDataModal from 'modules/MyProfile/EditProfile/EditUserData';
import MyProfileTemplate from 'templates/MyProfileTemplate';
import Icon from 'components/atoms/Icon';
import UserInfoBackground from 'components/molecules/UserInfoBackground';
import { Role } from 'utils/role';
import breakPointSize from 'utils/rwd';
import Nav from 'components/atoms/Nav';
import { Tabs } from 'antd';
import { trainerTabs, clientTabs} from '../Users/ProfileTabs'

const { TabPane } = Tabs;

const Container = styled.div`
  margin: auto;
  width: 74%;

  @media screen and ${breakPointSize.xs} {
    width: 100%;

    ${({ type }) => type === 'entry' && css`
      width: calc(100% - 3.2rem);
      margin: 0 1.6rem;
    `
  }
  }
`;

const ContainerCentred = styled.div`
  margin-top: 4.8rem;
  margin-bottom: 1.2rem;
`;

const TabItemComponent = ({
  icon = '',
  title = '',
  onItemClicked = () => console.error('You passed no action to the component'),
  isActive = false,
}) => {
  return (
    <div className={isActive ? 'tabitem' : 'tabitem tabitem--inactive'} onClick={onItemClicked}>
      <i className={icon}></i>
      <p className="tabitem__title">{title}</p>
    </div>
  )
};

const MyProfile = ({ setUser, toggleTheme, toggleLanguage }) => {
  const { theme } = useThemeContext();
  const [toRender, setToRender] = useState(null);

  const user = JSON.parse((localStorage.getItem('user')));
  const [updatedUser, setUpdatedUser] = useState(user);

  useEffect(() => {
    setUpdatedUser(user);
    getUserById();
  }, []);

  const getUserById = () => {
    userService
      .getUserById(user.userId)
      .then((data) => {
        setUpdatedUser(data);
      })
      .catch((error) => {
      });
  };

  const [bottomSheet, setBottomSheet] = useState('none');
  const [openEditUserData, setOpenEditUserData] = useState(false);
  const [openEditMailModal, setOpenEditMailModal] = useState(false);
  const [openEditUserPasswordModal, setOpenEditUserPasswordModal] = useState(false);
  const role = user.role.name;

  const Wrapper = styled.div`
      display: flex;
      justify-content: flex-start;
      padding: 1rem 0;
      color: ${({ theme }) => theme.colorGray10};

      &:hover {
          cursor: pointer;
      }
    `;

  const selectRole = () => {
    return role === Role.User ? clientTabs : trainerTabs;
  }

  const [active, setActive] = useState(0);

  return (
    <>
      <MyProfileTemplate>
        <UserInfoBackground>
          <Container>
            <Nav>
              <Wrapper>
                <Icon fill={theme.colorGray10} name="cog" size="2rem" onClick={() => setBottomSheet(!bottomSheet)} />
              </Wrapper>
            </Nav>
            <ContainerCentred>
              <UserInfo user={updatedUser} />
            </ContainerCentred>


            <div className="tabs">
              {selectRole().map(({ id, icon, title }) => <TabItemComponent
                key={title}
                icon={icon}
                title={title}
                onItemClicked={() => setActive(id)}
                isActive={active === id}
              />
              )}
            </div>
            <div className="content">
              {trainerTabs.map(({ id, content }) => {
                return active === id ? content : ''
              })}
            </div>


          </Container>
        </UserInfoBackground>
        <Container type="entry">
          {toRender}
        </Container>
      </MyProfileTemplate>
      <EditUserDataModal
        id={user.userId}
        openModal={openEditUserData}
        onClose={() => setOpenEditUserData(false)}
      />
      <EditUserEmailModal
        id={user.userId}
        openModal={openEditMailModal}
        onClose={() => setOpenEditMailModal(false)}
      />
      <EditUserPasswordModal
        id={user.userId}
        openModal={openEditUserPasswordModal}
        onClose={() => setOpenEditUserPasswordModal(false)}
      />
      <MyProfilePanel
        setUser={setUser}
        toggleTheme={toggleTheme}
        toggleLanguage={toggleLanguage}
        userId={user.userId}
        setOpenEditUserData={setOpenEditUserData}
        setOpenEditMailModal={setOpenEditMailModal}
        setOpenEditUserPasswordModal={setOpenEditUserPasswordModal}
        theme={theme}
        bottomSheet={bottomSheet}
        setBottomSheet={setBottomSheet}
      />
    </>
  );
};

export default MyProfile;
