import React, { useState } from 'react';
import styled from 'styled-components';
import ReturnWithTitle from 'components/molecules/ReturnWithTitle';
import { translate } from 'utils/Translation';
import ExerciseTemplate from 'templates/ExerciseTemplate';
import Button from 'components/atoms/Button';
import Paragraph from 'components/atoms/Paragraph';
import Label from 'components/atoms/Label';
import Input from 'components/molecules/Input';
import Icon from 'components/atoms/Icon';
import { Formik, Field, Form } from 'formik';
import * as Yup from 'yup';
import ErrorMessageForm from 'components/atoms/ErrorMessageForm';
import TextArea from 'components/molecules/TextArea';
import ImagePreview from 'components/molecules/ImagePreview';
import Random from 'utils/Random';
import { useNotificationContext, ADD } from 'support/context/NotificationContext';

const ContainerTopBeam = styled.div`
  display: flex;
  justify-content: space-between;
  margin-bottom: 2rem;
`;

const WrapperAttachments = styled.div`
  display: flex;
  margin-top: 2.2rem;
`;

const StyledParagraph = styled(Paragraph)`
  line-height: 0;
  margin: .8rem 0 0 .5rem;
`;

const StyledTextArea = styled(TextArea)`
  height: 28.3rem;
`;

const ContainerDescription = styled.div`
  margin-top: 2rem;
`;

const FileUploadButton = styled.input.attrs({ type: 'file' })`
  display: none;
`;

const ImagePreviewContainer = styled.div`
   margin-top: 2rem;
   display: grid;
   grid-template-columns: repeat(4, 5rem);
   grid-template-rows: 5rem;
   grid-gap: .8rem;
`;

const triggerFileUploadButton = () => {
  document.getElementById('choose-file-button').click();
};

const initialValues = {
  exerciseName: '',
  exerciseDescription: '',
};

const validationSchema = Yup.object({
  exerciseName: Yup.string().required(translate('ThisFieldIsRequired')),
  exerciseDescription: Yup.string(),
});

const onSubmit = (values) => {
  console.log(values);
};

const AddExerciseRefactor = () => {
  const [selectedFiles, setSelectedFiles] = useState([]);
  const { notificationDispatch } = useNotificationContext();

  const resetFileInput = () => {
    document.getElementById('choose-file-button').value = '';
  };

  const handleImageChange = (e) => {
    /* if (e.target.files) {
      const filesArray = Array.from(e.target.files).map((file) => URL.createObjectURL(file));

      setSelectedFiles((prevImages) => prevImages.concat(filesArray));
      Array.from(e.target.files).map(
        (file) => URL.revokeObjectURL(file), // avoid memory leak
      );
    } */

    // 'video/mov', 'video/wmv', 'video/fly', 'video/avi', 'video/avchd', 'webm', 'mkv'
    const acceptedImageFileType = ['image/jpeg', 'image/jpg', 'image/png', 'image/gif'];
    const acceptedVideoFileType = ['video/mp4'];

    // 32 mb
    const maxPhotoSize = 32000000;
    // 250 mb
    const maxVideoSize = 250000000;

    if (e.target.files) {
      Array.from(e.target.files).map((File) => {
        const fileType = File.type;
        const fileSize = File.size;

        // checking if the photo file type is correct
        if (acceptedImageFileType.includes(fileType)) {
          console.log('File size ', fileSize);
          // creating file object with unique ID
          const ID = Random(1, 10000);
          const fileData = {
            ID,
            File: URL.createObjectURL(File),
          };
          // append file object to state
          setSelectedFiles(((prevState) => prevState.concat(fileData)));

          // checking if the video file type is correct
        } else if (acceptedVideoFileType.includes(fileType)) {

        } else {
          // show alert
          notificationDispatch({
            type: ADD,
            payload: {
              content: { success: 'OK', message: 'Invalid file type' },
              type: 'error',
            },
          });
          resetFileInput();
        }
      });
    }
  };

  function removeFile(e) {
    e.stopPropagation();

    // get id of attachment preview
    const id = e.target.id.split('img-prev-')[1];

    // remove attachment
    for (let i = 0; i <= selectedFiles.length; ++i) {
      if (id == selectedFiles[i].ID) {
        const list = [...selectedFiles];
        const updatedList = list.filter((item) => item.ID !== selectedFiles[i].ID);
        setSelectedFiles(updatedList);

        resetFileInput();
        break;
      }
    }
  }

  const renderAttachmentsPreview = (source) => {
    if (source.length > 0) {
      return (
        <ImagePreviewContainer id="image-preview-container">
          {source.map((photo) => (
            <ImagePreview
              imageSrc={photo.File}
              alt=""
              key={photo.ID}
              setID={photo.ID}
              remove={removeFile}
              complete
            />
          ))}
        </ImagePreviewContainer>
      );
    }
  };

  const removeFile = (e) => {

  };

  return (
    <ExerciseTemplate>
      {/* eslint-disable-next-line max-len */}
      <Formik
        initialValues={initialValues}
        validationSchema={validationSchema}
        onSubmit={onSubmit}
        validateOnChange={false}
      >
        {({ errors, touched, isValid }) => (
          <Form>
            <ContainerTopBeam>
              <ReturnWithTitle text={translate('AddExercise')} />
              <Button size="sm" buttonType="primary" type="submit" disabled={!isValid}>{translate('Save')}</Button>
            </ContainerTopBeam>
            <Paragraph type="body-3-regular">{translate('AddExerciseInfo')}</Paragraph>
            <Label text={translate('ExerciseName')}>
              <Field type="text" name="exerciseName" as={Input} error={errors.exerciseName && touched.exerciseName} />
            </Label>
            <ErrorMessageForm name="exerciseName" />
            <WrapperAttachments onClick={triggerFileUploadButton}>
              <Icon name="image-plus" fill="white" height="1.5rem" width="1.5rem" />
              <StyledParagraph>{translate('AddAttachments')}</StyledParagraph>
              <FileUploadButton id="choose-file-button" onChange={handleImageChange} multiple />
            </WrapperAttachments>
            {renderAttachmentsPreview(selectedFiles)}
            <ContainerDescription>
              <Label text={translate('AddExerciseDescription')}>
                <Field type="text" name="exerciseDescription" as={StyledTextArea} />
              </Label>
            </ContainerDescription>
          </Form>
        )}
      </Formik>
    </ExerciseTemplate>
  );
};

export default AddExerciseRefactor;
