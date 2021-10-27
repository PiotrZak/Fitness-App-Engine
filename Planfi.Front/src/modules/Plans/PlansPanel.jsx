import React from 'react';
import Icon from 'components/atoms/Icon';
import { translate } from 'utils/Translation';
import styled from 'styled-components';
import { isMobile } from "react-device-detect";
import "react-multi-carousel/lib/styles.css";
import StyledReactBottomSheet, { PanelContainer, PanelItem } from 'components/organisms/BottomSheet'
import EditPlanModal from "./EditPlanModal";

const IconWrapper = styled.div`
    margin-top: .4rem;
`;

const PlansPanel = ({
    deletePlans,
    openEditModal,
    setOpenEditModal,
    theme,
    bottomSheet,
    setBottomSheet,
    selectedPlans
}) => {

    const closeModal = () => {
        setOpenEditModal(false)
        setBottomSheet('none')
    }

    const openModalFunction = () => {
        setOpenEditModal(true)
        setBottomSheet('none')
    }


    return (
        <StyledReactBottomSheet
            showBlockLayer={false}
            visible={bottomSheet}
            className={""}
            onClose={() => setBottomSheet(false)}
            appendCancelBtn={false}>
            {isMobile ?
                <>
                    <PanelItem onClick={() => deletePlans()}>
                        {selectedPlans.length == 1
                            ? <p>{translate('DeletePlan')}</p>
                            : <p>{translate('DeletePlans')}</p>
                        }
                    </PanelItem>
                    {selectedPlans.length < 2 &&
                        <PanelItem onClick={openModalFunction}>
                            <p>{translate('EditPlan')}</p>
                        </PanelItem>
                    }
                </>
                :
                <>
                    <PanelContainer>
                        <PanelItem>
                            <IconWrapper>
                                <Icon name="check" fill={theme.colorInputActive} />
                            </IconWrapper>
                            {selectedPlans.length} {translate('selected')}
                        </PanelItem>
                        <PanelItem onClick={() => deletePlans()}>
                            {translate('DeletePlan')}
                        </PanelItem>
                        {selectedPlans.length < 2 &&
                            <PanelItem onClick={openModalFunction}>
                                {translate('EditPlan')}
                            </PanelItem>
                        }
                    </PanelContainer>
                </>
            }
            <EditPlanModal
                selectedPlans={selectedPlans[0]}
                theme={theme}
                openEditModal={openEditModal}
                onClose={closeModal} />
        </StyledReactBottomSheet>
    )
}

export default PlansPanel;