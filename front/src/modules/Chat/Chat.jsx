import React, { useState, useEffect } from "react";
import { connect } from "react-redux";
import ChatRoomList from "./ChatRoom";
import { setUserName } from "../../store/actions/userActions";
import { receiveMessage } from "../../store/actions/messageActions";
import { HubConnectionBuilder } from "@microsoft/signalr";
import MessageList from "./ChatMessages";
import { useSelector } from "react-redux";
import AddMessageForm from "./AddMessageForm";

const hubUrl = "http://localhost:5005/chat";

const ChatContainer = () => {
  const [connection, setConnection] = useState();

  useEffect(() => {
    const connection = new HubConnectionBuilder().withUrl(hubUrl).build();
    setConnection(connection);

    connection
      .start({ withCredentials: false})
      .catch((err) => console.error(err.toString()));
      
  }, []);

  const currentRoom = useSelector((state) => state.requestRooms.currentRoom);

  return (
    <div className="panel panel-default">
      {connection && (
        <ChatRoomList openRoom={() => 1} connection={connection} />
      )}

      {currentRoom ? (
        <MessageList roomId={currentRoom.id} connection={connection} />
      ) : (
        <p>No room selected</p>
      )}

      {currentRoom && (
        <AddMessageForm
          roomId={currentRoom.id}
          userName={"userName"}
          connection={connection}
        />
      )}

      {/* {userName ? (

      ) : currentRoom ? (
        <UserNameForm onSetUserName={onSetUserName} />
      ) : (
        <div> Pick a room.</div>
      )}
      <AddChatRoomForm connection={this.connection} />  */}
    </div>
  );
};

export default ChatContainer;
