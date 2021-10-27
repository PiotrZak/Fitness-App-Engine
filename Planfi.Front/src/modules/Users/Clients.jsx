import React, { useEffect, useState, useCallback } from 'react';
import { organizationService } from 'services/organizationServices';
import { CheckboxGenericComponent } from 'components/organisms/CheckboxGeneric';
import GlobalTemplate from 'templates/GlobalTemplate';
import { useThemeContext } from 'support/context/ThemeContext';
import { translate } from 'utils/Translation';
import { useNotificationContext, ADD } from 'support/context/NotificationContext';
import Nav from 'components/atoms/Nav';
import Search from 'components/molecules/Search';
import Heading from 'components/atoms/Heading';
import Loader from 'components/atoms/Loader';
import styled from 'styled-components';
import { userService } from 'services/userServices';
import { useQuery, gql } from '@apollo/client';
import { Role } from 'utils/role';
import { commonUtil } from 'utils/common.util';
import InviteUserModal from './InviteUsersModal';
import SmallButton from 'components/atoms/SmallButton';
import { ClientPanel } from './ClientPanel';
import { UsersPanel } from './micromodules/UsersPanel';
import { AssignUsersToTrainers } from './micromodules/AssignUsersToTrainers';
import { AssignUsersToPlans } from './micromodules/AssignUsersToPlan';
import { filterDataByTerm } from '../../utils/common.util';

const Container = styled.div`
  text-align: center;
`;



const Clients = () => {


  // 
  const user = JSON.parse((localStorage.getItem('user')));
  const { theme } = useThemeContext();
  const { notificationDispatch } = useNotificationContext();




  const [isLoading, setIsLoading] = useState(false);
  const [searchTerm, setSearchTerm] = useState('');
  const [openInviteUserModal, setOpenInviteUserModal] = useState(false);
  const [activeUsers, setActiveUsers] = useState([]);


  //bottom panel logic

  const invisible = 'none';
  const visible = 'flex';

  const [bottomSheet, setBottomSheet] = useState(invisible);
  const [assignPlan, setAssignPlan] = useState(invisible);
  const [assignTrainer, setAssignTrainer] = useState(invisible);



  const [refresh, setRefresh] = useState(false)

  const Clients = gql`{
    users(where: {organizationId: "${user.organizationId}"})
    {
        userId
        avatar
        firstName
        lastName
     }
    }
  `;

  const {
    loading, error, data, refetch: _refetch,
  } = useQuery(Clients);
  const refreshData = useCallback(() => { setTimeout(() => _refetch(), 200); }, [_refetch]);

  const assignUserToPlan = useCallback((activeUsers, activePlans) => {
    const data = { clientIds: activeUsers, planIds: [activePlans] };

    refreshView()
    userService
      .assignPlanToUser(data)
      .then(() => {
        notificationDispatch({
          type: ADD,
          payload: {
            content: { success: 'OK', message: translate('PlansAssignedToUser') },
            type: 'positive'
          }
        })
      })
      .catch((error) => {
        notificationDispatch({
          type: ADD,
          payload: {
            content: { error: error, message: error.data.messages[0].text },
            type: 'warning'
          }
        })
      });
  }, []);

  const deleteUser = useCallback((activeUsers) => {
    userService
      .deleteUsers(activeUsers)
      .then(() => {
        notificationDispatch({
          type: ADD,
          payload: {
            content: { success: 'OK', message: translate('UserDeleted') },
            type: 'positive',
          },
        });
      })
      .catch((error) => {
        notificationDispatch({
          type: ADD,
          payload: {
            content: { error, message: translate('ErrorAlert') },
            type: 'warning'
          },
        });
      });
  }, []);

  const refreshView = () => {
    setAssignTrainer('none')
    setBottomSheet('none');
    setRefresh(!refresh)
  }

  const assignUserToMe = useCallback((activeUsers, activeTrainers) => {

    const data = { userIds: activeUsers, trainerIds: [user.userId] };
    refreshView()

    userService
      .assignUsersToTrainer(data)
      .then(() => {
        notificationDispatch({
          type: ADD,
          payload: {
            content: { success: 'OK', message: translate('TrainersAssignedToUser') },
            type: 'positive'
          }
        })

      })
      .catch((error) => {
        notificationDispatch({
          type: ADD,
          payload: {
            content: { error: error, message: error.data.messages[0].text },
            type: 'warning'
          }
        })
      });
  }, []);

  const assignUserToTrainer = useCallback((activeUsers, activeTrainers) => {

    const data = { userIds: activeUsers, trainerIds: [activeTrainers] };
    refreshView()

    userService
      .assignUsersToTrainer(data)
      .then(() => {
        notificationDispatch({
          type: ADD,
          payload: {
            content: { success: 'OK', message: translate('TrainersAssignedToUser') },
            type: 'positive'
          }
        })
        setRefresh(!refresh)
      })
      .catch((error) => {
        notificationDispatch({
          type: ADD,
          payload: {
            content: { error: error, message: error.data.messages[0].text },
            type: 'warning'
          }
        })
      });
  }, []);

  useEffect(() => {
    refreshData();
  }, [refreshData])

  const submissionHandleElement = (selectedData) => {
    const selectedUsers = commonUtil.getCheckedData(selectedData, 'userId');
    setActiveUsers(selectedUsers);
    setAssignTrainer('flex');
  };

  let results;
  if (data) {
    results = filterDataByTerm(searchTerm, data.users, ['firstName', 'lastName']);
  }
  if (loading) return <Loader isLoading={loading} />;
  if (error) return <p>Error :(</p>;

  return (
    <>
      <GlobalTemplate>
        
        <Nav>
          <Heading>{translate('Clients')}</Heading>
          <SmallButton iconName="plus" onClick={() => setOpenInviteUserModal(true)} />
        </Nav>
        <InviteUserModal role={Role.User} openModal={openInviteUserModal} onClose={() => setOpenInviteUserModal(false)} />
        <Container>
          <Search placeholder={translate('Find')} callBack={(e) => setSearchTerm(e.target.value)} />
        </Container>


        <Loader isLoading={isLoading}>
          {results.filter(x => x.userId != user.userId).length > 0
            ? (
              <CheckboxGenericComponent
                dataType="users"
                displayedValue="firstName"
                dataList={results.filter(x => x.userId != user.userId)}
                onSelect={submissionHandleElement}
                refresh={refresh}
              />
            )
            : <p>{translate('NoUsers')}</p>}
        </Loader>
        {user.role.name}
      </GlobalTemplate>

      {user.role && user.role.name !== Role.Owner &&
      <>
        <ClientPanel

          theme={theme}
          userId={user.userId}
          organizationId={user.organizationId}

          assignPlan={assignPlan}
          assignUserToPlan={assignUserToPlan}
          setAssignPlan={setAssignPlan}

          assignTrainer={assignTrainer}
          assignUserToTrainer={assignUserToTrainer}
          setAssignTrainer={setAssignTrainer}

           assignUserToMe={assignUserToMe}

          setBottomSheet={setBottomSheet}
          activeUsers={activeUsers}
        />
        </>}
    </>
  );
};

export default Clients;