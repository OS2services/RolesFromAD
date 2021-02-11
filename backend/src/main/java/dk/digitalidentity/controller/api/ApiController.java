package dk.digitalidentity.controller.api;

import java.util.List;
import java.util.stream.Collectors;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

import dk.digitalidentity.config.AppConfiguration;
import dk.digitalidentity.controller.api.dto.UserDTO;
import dk.digitalidentity.service.ADBridgeService;
import dk.digitalidentity.service.model.CommandResponse;
import lombok.extern.slf4j.Slf4j;

@Slf4j
@RestController
public class ApiController {

	@Autowired
	private AppConfiguration config;

	@Autowired
	private ADBridgeService adBridgeService;

	@GetMapping("/api/whoHasRole")
	public ResponseEntity<?> whoHasRole(@RequestParam(name = "identifier", required = true) String identifier) {
		log.info("whoHasRole = " + identifier + " called");

		List<UserDTO> list = null;
		try {
			String roleIdentifier = mapIdentifier(identifier);

			if (!config.isEnabled()) {
				log.info("Returning stubbed result");
				list = config.getTestData();
			}
			else {
				CommandResponse response = adBridgeService.getUsersWithRole(roleIdentifier);
				list = response.getPayload();
			}
		}
		catch (Exception ex) {
			log.error("Unexpected error", ex);
			return ResponseEntity.badRequest().body(ex.getMessage());
		}

		log.info("Found " + list.size() + " users with role: " + identifier);

		return ResponseEntity.ok(list);
	}
	
	private String mapIdentifier(String identifier) throws Exception {
		List<String> roleIdentifiers = config.getMapping().stream()
				.filter(m -> m.getIdentifier().equals(identifier))
				.map(m -> m.getGroupIdentifier())
				.collect(Collectors.toList());

		if (roleIdentifiers.size() == 0) {
			throw new Exception("No group mapping configured for " + identifier);
		}
		else if (roleIdentifiers.size() > 1) {
			throw new Exception("More than one group mapping found for " + identifier);
		}

		return roleIdentifiers.get(0);
	}
}
