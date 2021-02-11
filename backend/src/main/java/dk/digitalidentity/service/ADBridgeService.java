package dk.digitalidentity.service;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.scheduling.annotation.AsyncResult;
import org.springframework.stereotype.Service;

import dk.digitalidentity.service.model.CommandResponse;

@Service
public class ADBridgeService {

	@Autowired
	private SocketHandler socketHandler;

	public CommandResponse getUsersWithRole(String identifier) throws Exception {
		AsyncResult<CommandResponse> result = socketHandler.getUsersWithRole(identifier);
		
		CommandResponse response = result.get();
		if (!response.isValid()) {
			throw new Exception("Unable to get data from Active Direectory");
		}
		
		return response;
	}
}
