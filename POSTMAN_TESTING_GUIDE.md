
All requests should be configured as follows:
- **Method**: `POST`
- **URL**: `http://localhost:53863/graphql`
- **Headers**:
  ```
  Content-Type: application/json
  ```

## Query Examples

### 1. Get All Companies

**Body**:
```json
{
  "query": "query GetCompanies { companies { id name description industry location website employeeCount foundedDate notes } }"
}
```

### 2. Get Company by ID

**Body**:
```json
{
  "query": "query GetCompany($id: Int!) { company(id: $id) { id name description industry location website employeeCount foundedDate notes } }",
  "variables": {
    "id": 1
  }
}
```

### 3. Get All Projects

**Body**:
```json
{
  "query": "query GetProjects { projects { id name description technologies startDate endDate projectUrl sourceCodeUrl isFeatured isOngoing technologiesList } }"
}
```

### 4. Get Project by ID

**Body**:
```json
{
  "query": "query GetProject($id: Int!) { project(id: $id) { id name description technologies startDate endDate projectUrl sourceCodeUrl isFeatured isOngoing technologiesList } }",
  "variables": {
    "id": 1
  }
}
```

### 5. Get All Education Records

**Body**:
```json
{
  "query": "query GetEducations { educations { id institution degree fieldOfStudy startDate endDate grade description isInProgress } }"
}
```

### 6. Get Education by ID

**Body**:
```json
{
  "query": "query GetEducation($id: Int!) { education(id: $id) { id institution degree fieldOfStudy startDate endDate grade description isInProgress } }",
  "variables": {
    "id": 1
  }
}
```

### 7. Get All Skills

**Body**:
```json
{
  "query": "query GetSkills { skills { id name proficiencyLevel category yearsOfExperience description } }"
}
```

### 8. Get Skill by ID

**Body**:
```json
{
  "query": "query GetSkill($id: Int!) { skill(id: $id) { id name proficiencyLevel category yearsOfExperience description } }",
  "variables": {
    "id": 1
  }
}
```